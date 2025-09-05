using Microsoft.AspNetCore.Mvc;

namespace ChatAppWithSignalR.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ChatController : ControllerBase
{
    public ChatController()
    {
        
    }
    [HttpPost]
    public IActionResult SendChat()
    {
        return Ok();
    }
}