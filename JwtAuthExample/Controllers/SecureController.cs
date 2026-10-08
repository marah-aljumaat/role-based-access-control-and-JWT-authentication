using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/secure")]
public class SecureController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetSecureData()
    {
        return Ok(new { Message = "This is a secure endpoint." });
    }
}