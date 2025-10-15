using SpeakEasy.Api.Models;

namespace SpeakEasy.Api.DTOs;

public class InitiateKeyExchangeDto
{
    public int ReceiverId { get; set; }
    public required string PublicKey { get; set; }
}

public class AcceptKeyExchangeDto
{
    public int KeyExchangeId { get; set; }
    public required string PublicKey { get; set; }
}

public class KeyExchangeDto
{
    public int Id { get; set; }
    public int InitiatorId { get; set; }
    public required string InitiatorUsername { get; set; }
    public int ReceiverId { get; set; }
    public required string ReceiverUsername { get; set; }
    public required string InitiatorPublicKey { get; set; }
    public string? ReceiverPublicKey { get; set; }
    public KeyExchangeStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
