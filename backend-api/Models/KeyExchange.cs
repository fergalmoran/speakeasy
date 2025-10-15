namespace SpeakEasy.Api.Models;

public class KeyExchange
{
    public int Id { get; set; }
    public int InitiatorId { get; set; }
    public User Initiator { get; set; } = null!;
    public int ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    public required string InitiatorPublicKey { get; set; }
    public string? ReceiverPublicKey { get; set; }
    public KeyExchangeStatus Status { get; set; } = KeyExchangeStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

public enum KeyExchangeStatus
{
    Pending,
    Accepted,
    Rejected,
    Completed
}
