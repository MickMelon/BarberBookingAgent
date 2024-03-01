namespace BarberBookingAgent.Core.Entities;

public class AppointmentEntity
{
    public int Id { get; set; }

    public string Barber { get; set; } = string.Empty;

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public bool IsBooked { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerPhoneNumber { get; set; }

    public string? CustomerEmail { get; set; }
}
