using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace BarberBookingAgent.WebApi.Plugins;

public record Appointment(string Barber, DateTime Start, DateTime End);

public class BookingPlugin
{
    private readonly List<Appointment> _appointments = new();

    public BookingPlugin()
    {
        PopulateAppointmentsRandomly(100);
    }

    public void PopulateAppointmentsRandomly(int numberOfAppointments)
    {
        Random rand = new();
        List<string> barbers = new() { "John", "Paul", "George", "Ringo" };
        List<int> startHours = Enumerable.Range(8, 12).ToList(); // Assuming business hours from 8 AM to 8 PM

        for (int i = 0; i < numberOfAppointments; i++)
        {
            int barberIndex = rand.Next(barbers.Count);
            int dayOffset = rand.Next(1, 30); // Random day within the next 30 days
            int hourOffset = startHours[rand.Next(startHours.Count)]; // Random hour within business hours
            int appointmentDurationHours = rand.Next(1, 3); // Random duration between 1 and 2 hours

            DateTime startDateTime = DateTime.Now.AddDays(dayOffset).Date.AddHours(hourOffset);
            DateTime endDateTime = startDateTime.AddHours(appointmentDurationHours);

            Appointment appointment = new(barbers[barberIndex], startDateTime, endDateTime);
            _appointments.Add(appointment);
        }
    }

    [KernelFunction]
    [Description("Lists all barber names.")]
    [return: Description("A list of names of all the barbers.")]
    public async Task<List<string>> ListAllBarberNamesAsync()
    {
        Console.WriteLine("Barbers listed!");

        return new() { "John", "Paul", "George", "Ringo" };
    }

    [KernelFunction]
    [Description("Books an appointment.")]
    public async Task BookAppointmentAsync(
        [Description("The name of the barber to book an appointment with.")] string barber,
        [Description("The date and time to book the appointment.")] string dateTime)
    {
        Console.WriteLine($"Appointment booked with {barber} on {dateTime}!");
    }

    [KernelFunction]
    [Description("Lists all available appointments within the next seven days.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<Appointment>> ListAppointmentsForNextSevenDaysAsync()
    {
        Console.WriteLine("Available dates listed");

        return _appointments
            .Where(_ => _.Start < DateTime.Now.AddDays(7))
            .ToList();
    }

    [KernelFunction]
    [Description("Lists all available appointments within the next seven days for a specific barber.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<Appointment>> ListAppointmentsForNextSevenDaysForBarberAsync(
        [Description("The barber's name.")] string? barber)
    {
        Console.WriteLine("Available dates listed");

        return _appointments
            .Where(_ => _.Barber.Equals(barber, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    [KernelFunction]
    [Description("Lists all available appointments for a specific date.")]
    [return: Description("A list of available appointments.")]
    public async Task<List<Appointment>> ListAppointmentsForDateAsync(
        [Description("The date. Must be valid DateTime format parseable by C# DateTime.Parse")] string date)
    {
        Console.WriteLine("Available dates listed");

        DateTime dateTime = DateTime.Parse(date);

        return _appointments
            .Where(_ => _.Start.Date == dateTime.Date)
            .ToList();
    }
}
