using System.ComponentModel;
using BarberBookingAgent.Application.Common.Interfaces;
using BarberBookingAgent.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace BarberBookingAgent.Application.Plugins;

public class BookingPlugin
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<BookingPlugin> _logger;

    public BookingPlugin(
        IApplicationDbContext dbContext,
        ILogger<BookingPlugin> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [KernelFunction]
    [Description("Lists all barber names.")]
    [return: Description("A list of names of all the barbers.")]
    public async Task<List<string>> ListAllBarberNamesAsync()
    {
        _logger.LogInformation("Barbers listed.");

        return new() { "John", "Paul", "George", "Ringo" };
    }

    [KernelFunction]
    [Description("Books an appointment.")]
    public async Task<string> BookAppointmentAsync(
        [Description("The appointment ID.")] int appointmentId,
        [Description("The customer's name.")] string customerName,
        [Description("The customer's phone number.")] string customerPhoneNumber,
        [Description("The customer's email.")] string customerEmail)
    {
        AppointmentEntity? appointment = await _dbContext
            .Appointments
            .FirstOrDefaultAsync(_ => _.Id == appointmentId);

        if (appointment is null)
        {
            _logger.LogInformation("Failed to book appointment: not found");
            return "Appointment not found";
        }

        appointment.IsBooked = true;
        appointment.CustomerName = customerName;
        appointment.CustomerPhoneNumber = customerPhoneNumber;
        appointment.CustomerEmail = customerEmail;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Appointment {AppointmentId} booked.", appointmentId);

        return "Appointment booked.";
    }

    [KernelFunction]
    [Description("Cancels an appointment.")]
    public async Task<string> CancelAppointmentAsync(
        [Description("The appointment ID.")] int appointmentId)
    {
        AppointmentEntity? appointment = await _dbContext
            .Appointments
            .FirstOrDefaultAsync(_ => _.Id == appointmentId);

        if (appointment is null)
        {
            _logger.LogInformation("Failed to cancel appointment: not found");
            return "Appointment not found";
        }

        if (!appointment.IsBooked)
        {
            _logger.LogInformation("Failed to cancel appointment: not booked");
            return "Appointment not booked";
        }

        appointment.IsBooked = false;
        appointment.CustomerName = null;
        appointment.CustomerPhoneNumber = null;
        appointment.CustomerEmail = null;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Appointment {AppointmentId} cancelled.", appointmentId);

        return "Appointment cancelled.";
    }

    [KernelFunction]
    [Description("Lists all available appointments within the next seven days.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<AppointmentEntity>> ListAppointmentsForNextSevenDaysAsync()
    {
        List<AppointmentEntity> appointments = await _dbContext.Appointments.ToListAsync();

        _logger.LogInformation("Available dates listed");

        return appointments
            .Where(_ => _.Start < DateTime.Now.AddDays(7))
            .ToList();
    }

    [KernelFunction]
    [Description("Lists all available appointments within the next seven days for a specific barber.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<AppointmentEntity>> ListAppointmentsForNextSevenDaysForBarberAsync(
        [Description("The barber's name.")] string? barber)
    {
        List<AppointmentEntity> appointments = await _dbContext.Appointments.ToListAsync();

        _logger.LogInformation("Available dates listed");

        return appointments
            .Where(_ => _.Barber.Equals(barber, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    [KernelFunction]
    [Description("Lists all available appointments for a specific date.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<AppointmentEntity>> ListAppointmentsForDateAsync(
        [Description("The date. Must be valid DateTime format parseable by C# DateTime.Parse")] string date)
    {
        List<AppointmentEntity> appointments = await _dbContext.Appointments.ToListAsync();

        _logger.LogInformation("Available dates listed");

        DateTime dateTime = DateTime.Parse(date);

        return appointments
            .Where(_ => _.Start.Date == dateTime.Date)
            .ToList();
    }
}
