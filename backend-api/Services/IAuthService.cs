using SpeakEasy.Api.DTOs;
using SpeakEasy.Api.Models;

namespace SpeakEasy.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> Register(RegisterDto dto);
    Task<AuthResponseDto?> Login(LoginDto dto);
    Task<UserDto?> GetUserById(int userId);
    Task<List<UserDto>> GetAllUsers(int currentUserId);
    Task<bool> UpdatePublicKey(int userId, string publicKey);
}
