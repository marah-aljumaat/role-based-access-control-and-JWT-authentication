using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var token = _tokenService.GenerateToken(request.Username, "Admin");
            return Ok(new { Token = token });
        }

        if (request.Username == "user" && request.Password == "user123")
        {
            var token = _tokenService.GenerateToken(request.Username, "User");
            return Ok(new { Token = token });
        }

        return Unauthorized("Invalid credentials");
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}