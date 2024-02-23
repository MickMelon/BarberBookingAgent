using BarberBookingAgent.WebApi.Extensions;
using BarberBookingAgent.WebApi.Options;
using BarberBookingAgent.WebApi.Storage;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<OpenAiOptions>()
    .Bind(builder.Configuration.GetSection(nameof(OpenAiOptions)))
    .ValidateDataAnnotations();

builder.Services.AddSemanticKernel();
builder.Services.AddSingleton<ChatHistoryStorage>();

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

app.Run();
