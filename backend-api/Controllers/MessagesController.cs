using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeakEasy.Api.DTOs;
using SpeakEasy.Api.Services;
using System.Security.Claims;

namespace SpeakEasy.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> SendMessage([FromBody] SendMessageDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var result = await _messageService.SendMessage(userId, dto);
        
        if (result == null)
            return BadRequest(new { message = "Receiver not found" });

        return Ok(result);
    }

    [HttpGet("conversations")]
    public async Task<ActionResult<List<ConversationDto>>> GetConversations()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var conversations = await _messageService.GetConversations(userId);
        
        return Ok(conversations);
    }

    [HttpGet("conversation/{otherUserId}")]
    public async Task<ActionResult<List<MessageDto>>> GetConversation(int otherUserId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var messages = await _messageService.GetConversation(userId, otherUserId);
        
        return Ok(messages);
    }

    [HttpPost("{messageId}/read")]
    public async Task<ActionResult> MarkAsRead(int messageId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var result = await _messageService.MarkAsRead(messageId, userId);
        
        if (!result)
            return BadRequest();

        return Ok();
    }
}
