using SpeakEasy.Api.DTOs;

namespace SpeakEasy.Api.Services;

public interface IMessageService
{
    Task<MessageDto?> SendMessage(int senderId, SendMessageDto dto);
    Task<List<MessageDto>> GetConversation(int userId, int otherUserId);
    Task<List<ConversationDto>> GetConversations(int userId);
    Task<bool> MarkAsRead(int messageId, int userId);
}
