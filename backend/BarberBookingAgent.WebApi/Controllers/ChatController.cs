using BarberBookingAgent.Application.Chat;
using Microsoft.AspNetCore.Mvc;

namespace BarberBookingAgent.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    [HttpPost("message")]
    public async Task<ActionResult<ChatMessageResponse>> Message(
        [FromBody] ChatMessageRequest request,
        [FromServices] ChatMessageHandler handler)
    {
        ChatMessageResponse response = await handler.Handle(request);

        return Ok(response);
    }
}
