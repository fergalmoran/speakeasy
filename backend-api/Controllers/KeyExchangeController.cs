using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeakEasy.Api.DTOs;
using SpeakEasy.Api.Services;
using System.Security.Claims;

namespace SpeakEasy.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class KeyExchangeController : ControllerBase
{
    private readonly IKeyExchangeService _keyExchangeService;

    public KeyExchangeController(IKeyExchangeService keyExchangeService)
    {
        _keyExchangeService = keyExchangeService;
    }

    [HttpPost("initiate")]
    public async Task<ActionResult<KeyExchangeDto>> InitiateKeyExchange([FromBody] InitiateKeyExchangeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var result = await _keyExchangeService.InitiateKeyExchange(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "Receiver not found" });

        return Ok(result);
    }

    [HttpPost("accept")]
    public async Task<ActionResult<KeyExchangeDto>> AcceptKeyExchange([FromBody] AcceptKeyExchangeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var result = await _keyExchangeService.AcceptKeyExchange(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "Key exchange not found" });

        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<KeyExchangeDto>>> GetPendingKeyExchanges()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var keyExchanges = await _keyExchangeService.GetPendingKeyExchanges(userId);
        
        return Ok(keyExchanges);
    }

    [HttpGet("user/{otherUserId}")]
    public async Task<ActionResult<KeyExchangeDto>> GetKeyExchange(int otherUserId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var keyExchange = await _keyExchangeService.GetKeyExchange(userId, otherUserId);
        
        if (keyExchange == null)
            return NotFound();

        return Ok(keyExchange);
    }
}
