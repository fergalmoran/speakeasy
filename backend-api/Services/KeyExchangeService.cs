using Microsoft.EntityFrameworkCore;
using SpeakEasy.Api.Data;
using SpeakEasy.Api.DTOs;
using SpeakEasy.Api.Models;

namespace SpeakEasy.Api.Services;

public class KeyExchangeService : IKeyExchangeService
{
    private readonly ApplicationDbContext _context;

    public KeyExchangeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<KeyExchangeDto?> InitiateKeyExchange(int initiatorId, InitiateKeyExchangeDto dto)
    {
        var receiver = await _context.Users.FindAsync(dto.ReceiverId);
        if (receiver == null) return null;

        // Check if already exists
        var existing = await _context.KeyExchanges
            .FirstOrDefaultAsync(k => 
                (k.InitiatorId == initiatorId && k.ReceiverId == dto.ReceiverId) ||
                (k.InitiatorId == dto.ReceiverId && k.ReceiverId == initiatorId));

        if (existing != null)
        {
            existing.InitiatorPublicKey = dto.PublicKey;
            existing.Status = KeyExchangeStatus.Pending;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            existing = new KeyExchange
            {
                InitiatorId = initiatorId,
                ReceiverId = dto.ReceiverId,
                InitiatorPublicKey = dto.PublicKey,
                Status = KeyExchangeStatus.Pending
            };
            _context.KeyExchanges.Add(existing);
        }

        await _context.SaveChangesAsync();

        var initiator = await _context.Users.FindAsync(initiatorId);
        return new KeyExchangeDto
        {
            Id = existing.Id,
            InitiatorId = existing.InitiatorId,
            InitiatorUsername = initiator!.Username,
            ReceiverId = existing.ReceiverId,
            ReceiverUsername = receiver.Username,
            InitiatorPublicKey = existing.InitiatorPublicKey,
            ReceiverPublicKey = existing.ReceiverPublicKey,
            Status = existing.Status,
            CreatedAt = existing.CreatedAt,
            CompletedAt = existing.CompletedAt
        };
    }

    public async Task<KeyExchangeDto?> AcceptKeyExchange(int receiverId, AcceptKeyExchangeDto dto)
    {
        var keyExchange = await _context.KeyExchanges
            .Include(k => k.Initiator)
            .Include(k => k.Receiver)
            .FirstOrDefaultAsync(k => k.Id == dto.KeyExchangeId && k.ReceiverId == receiverId);

        if (keyExchange == null) return null;

        keyExchange.ReceiverPublicKey = dto.PublicKey;
        keyExchange.Status = KeyExchangeStatus.Completed;
        keyExchange.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new KeyExchangeDto
        {
            Id = keyExchange.Id,
            InitiatorId = keyExchange.InitiatorId,
            InitiatorUsername = keyExchange.Initiator.Username,
            ReceiverId = keyExchange.ReceiverId,
            ReceiverUsername = keyExchange.Receiver.Username,
            InitiatorPublicKey = keyExchange.InitiatorPublicKey,
            ReceiverPublicKey = keyExchange.ReceiverPublicKey,
            Status = keyExchange.Status,
            CreatedAt = keyExchange.CreatedAt,
            CompletedAt = keyExchange.CompletedAt
        };
    }

    public async Task<List<KeyExchangeDto>> GetPendingKeyExchanges(int userId)
    {
        return await _context.KeyExchanges
            .Include(k => k.Initiator)
            .Include(k => k.Receiver)
            .Where(k => k.ReceiverId == userId && k.Status == KeyExchangeStatus.Pending)
            .Select(k => new KeyExchangeDto
            {
                Id = k.Id,
                InitiatorId = k.InitiatorId,
                InitiatorUsername = k.Initiator.Username,
                ReceiverId = k.ReceiverId,
                ReceiverUsername = k.Receiver.Username,
                InitiatorPublicKey = k.InitiatorPublicKey,
                ReceiverPublicKey = k.ReceiverPublicKey,
                Status = k.Status,
                CreatedAt = k.CreatedAt,
                CompletedAt = k.CompletedAt
            })
            .ToListAsync();
    }

    public async Task<KeyExchangeDto?> GetKeyExchange(int userId, int otherUserId)
    {
        var keyExchange = await _context.KeyExchanges
            .Include(k => k.Initiator)
            .Include(k => k.Receiver)
            .Where(k => k.Status == KeyExchangeStatus.Completed &&
                   ((k.InitiatorId == userId && k.ReceiverId == otherUserId) ||
                    (k.InitiatorId == otherUserId && k.ReceiverId == userId)))
            .FirstOrDefaultAsync();

        if (keyExchange == null) return null;

        return new KeyExchangeDto
        {
            Id = keyExchange.Id,
            InitiatorId = keyExchange.InitiatorId,
            InitiatorUsername = keyExchange.Initiator.Username,
            ReceiverId = keyExchange.ReceiverId,
            ReceiverUsername = keyExchange.Receiver.Username,
            InitiatorPublicKey = keyExchange.InitiatorPublicKey,
            ReceiverPublicKey = keyExchange.ReceiverPublicKey,
            Status = keyExchange.Status,
            CreatedAt = keyExchange.CreatedAt,
            CompletedAt = keyExchange.CompletedAt
        };
    }
}
