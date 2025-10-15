namespace SpeakEasy.Api.DTOs;

public class SendMessageDto
{
    public int ReceiverId { get; set; }
    public required string EncryptedContent { get; set; }
}

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public required string SenderUsername { get; set; }
    public int ReceiverId { get; set; }
    public required string ReceiverUsername { get; set; }
    public required string EncryptedContent { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class ConversationDto
{
    public required UserDto User { get; set; }
    public MessageDto? LastMessage { get; set; }
    public int UnreadCount { get; set; }
}
