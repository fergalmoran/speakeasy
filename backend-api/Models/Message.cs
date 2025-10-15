namespace SpeakEasy.Api.Models;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    public int ReceiverId { get; set; }
    public User Receiver { get; set; } = null!;
    public required string EncryptedContent { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
