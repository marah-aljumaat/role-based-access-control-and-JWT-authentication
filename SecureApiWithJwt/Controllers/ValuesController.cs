using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ValuesController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminData()
    {
        return Ok("This is Admin data");
    }

    [HttpGet("user")]
    [Authorize(Roles = "User")]
    public IActionResult GetUserData()
    {
        return Ok("This is User data");
    }
}