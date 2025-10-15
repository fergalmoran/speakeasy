using Microsoft.EntityFrameworkCore;
using SpeakEasy.Api.Data;
using SpeakEasy.Api.DTOs;
using SpeakEasy.Api.Models;

namespace SpeakEasy.Api.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;

    public MessageService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MessageDto?> SendMessage(int senderId, SendMessageDto dto)
    {
        var receiver = await _context.Users.FindAsync(dto.ReceiverId);
        if (receiver == null) return null;

        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            EncryptedContent = dto.EncryptedContent
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(senderId);
        return new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderUsername = sender!.Username,
            ReceiverId = message.ReceiverId,
            ReceiverUsername = receiver.Username,
            EncryptedContent = message.EncryptedContent,
            SentAt = message.SentAt,
            ReadAt = message.ReadAt
        };
    }

    public async Task<List<MessageDto>> GetConversation(int userId, int otherUserId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => !m.IsDeleted && 
                   ((m.SenderId == userId && m.ReceiverId == otherUserId) ||
                    (m.SenderId == otherUserId && m.ReceiverId == userId)))
            .OrderBy(m => m.SentAt)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderUsername = m.Sender.Username,
                ReceiverId = m.ReceiverId,
                ReceiverUsername = m.Receiver.Username,
                EncryptedContent = m.EncryptedContent,
                SentAt = m.SentAt,
                ReadAt = m.ReadAt
            })
            .ToListAsync();
    }

    public async Task<List<ConversationDto>> GetConversations(int userId)
    {
        var conversations = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => !m.IsDeleted && (m.SenderId == userId || m.ReceiverId == userId))
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g => new
            {
                OtherUserId = g.Key,
                LastMessage = g.OrderByDescending(m => m.SentAt).First(),
                UnreadCount = g.Count(m => m.ReceiverId == userId && m.ReadAt == null)
            })
            .ToListAsync();

        var result = new List<ConversationDto>();
        foreach (var conv in conversations)
        {
            var otherUser = await _context.Users.FindAsync(conv.OtherUserId);
            if (otherUser != null)
            {
                result.Add(new ConversationDto
                {
                    User = new UserDto
                    {
                        Id = otherUser.Id,
                        Username = otherUser.Username,
                        Email = otherUser.Email,
                        PublicKey = otherUser.PublicKey,
                        LastSeen = otherUser.LastSeen
                    },
                    LastMessage = new MessageDto
                    {
                        Id = conv.LastMessage.Id,
                        SenderId = conv.LastMessage.SenderId,
                        SenderUsername = conv.LastMessage.Sender.Username,
                        ReceiverId = conv.LastMessage.ReceiverId,
                        ReceiverUsername = conv.LastMessage.Receiver.Username,
                        EncryptedContent = conv.LastMessage.EncryptedContent,
                        SentAt = conv.LastMessage.SentAt,
                        ReadAt = conv.LastMessage.ReadAt
                    },
                    UnreadCount = conv.UnreadCount
                });
            }
        }

        return result.OrderByDescending(c => c.LastMessage?.SentAt).ToList();
    }

    public async Task<bool> MarkAsRead(int messageId, int userId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null || message.ReceiverId != userId) return false;

        message.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
