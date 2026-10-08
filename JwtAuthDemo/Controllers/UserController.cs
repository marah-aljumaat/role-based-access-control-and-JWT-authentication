using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly TokenService _tokenService;
    private static readonly List<User> _users = new List<User>
    {
        new User { Username = "testuser", Password = "password123" }
    };

    public UserController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    [HttpGet("secure-data")]
    [Authorize]
    public IActionResult GetSecureData()
    {
        return Ok(new { Message = "This is a protected endpoint!" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var existingUser = _users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);
        if (existingUser == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _tokenService.GenerateToken(user.Username);
        return Ok(new { Token = token });
    }
}