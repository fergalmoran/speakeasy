namespace SpeakEasy.Api.DTOs;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? PublicKey { get; set; }
}

public class LoginDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class AuthResponseDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public string? PublicKey { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? PublicKey { get; set; }
    public DateTime? LastSeen { get; set; }
}
