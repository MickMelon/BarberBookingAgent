namespace BarberBookingAgent.Core.Entities;

public class ChatMessageEntity
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string SessionId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
