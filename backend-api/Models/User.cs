namespace SpeakEasy.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? PublicKey { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastSeen { get; set; }
    
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<KeyExchange> InitiatedKeyExchanges { get; set; } = new List<KeyExchange>();
    public ICollection<KeyExchange> ReceivedKeyExchanges { get; set; } = new List<KeyExchange>();
}
