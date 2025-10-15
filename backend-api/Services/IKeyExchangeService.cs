using SpeakEasy.Api.DTOs;

namespace SpeakEasy.Api.Services;

public interface IKeyExchangeService
{
    Task<KeyExchangeDto?> InitiateKeyExchange(int initiatorId, InitiateKeyExchangeDto dto);
    Task<KeyExchangeDto?> AcceptKeyExchange(int receiverId, AcceptKeyExchangeDto dto);
    Task<List<KeyExchangeDto>> GetPendingKeyExchanges(int userId);
    Task<KeyExchangeDto?> GetKeyExchange(int userId, int otherUserId);
}
