using BarberBookingAgent.Application;
using BarberBookingAgent.Application.Common.Interfaces;
using BarberBookingAgent.Application.Common.Options;
using BarberBookingAgent.Application.Plugins;
using BarberBookingAgent.Core.Entities;
using BarberBookingAgent.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddOptions<OpenAiOptions>()
    .Bind(builder.Configuration.GetSection(nameof(OpenAiOptions)))
    .ValidateDataAnnotations();

builder.Services.AddScoped<Kernel>(serviceProvider =>
{
    OpenAiOptions options = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;

    IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.Services.AddLogging(_ => _
        .SetMinimumLevel(LogLevel.Trace)
        .AddDebug()
        .AddConsole());

    kernelBuilder.Services.AddOpenAIChatCompletion(options.ModelId, options.ApiKey, options.OrganizationId);

    kernelBuilder.Plugins.AddFromType<BookingPlugin>();

    // TODO: how to get it to use MS services
    kernelBuilder.Services.AddInfrastructure(builder.Configuration);

    Kernel kernel = kernelBuilder.Build();

    return kernel;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

IApplicationDbContext dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();

if (!dbContext.Appointments.Any())
{
    dbContext.Appointments.AddRange(GenerateAppointments());
    await dbContext.SaveChangesAsync();
}

app.Run();

static List<AppointmentEntity> GenerateAppointments()
{
    List<AppointmentEntity> appointments = new List<AppointmentEntity>();
    string[] barbers = { "John", "Paul", "Ringo", "George" };
    DateTime startDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
    DateTime endBusinessDay = startDateTime.AddHours(8); // Business hours end at 5 PM

    for (int i = 0; i < 50; i++)
    {
        var barber = barbers[i % 4]; // Cycle through the list of barbers
        var start = startDateTime.AddHours(i); // Increment the start time for each appointment
        var end = start.AddHours(1); // Assume each appointment is 1 hour

        // Ensure the appointment ends within business hours
        if (end.TimeOfDay > endBusinessDay.TimeOfDay)
        {
            startDateTime = startDateTime.AddDays(1).Date + new TimeSpan(9, 0, 0); // Move to the next day at 9 AM
            start = startDateTime;
            end = start.AddHours(1);
        }

        appointments.Add(new AppointmentEntity
        {
            Barber = barber,
            Start = start,
            End = end,
            IsBooked = false,
            CustomerName = null,
            CustomerPhoneNumber = null,
            CustomerEmail = null
        });
    }

    return appointments;
}
