using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private static readonly Dictionary<string, string> Users =
        new() { { "user1", "password1" }, { "admin", "password2" } };

    private readonly string secretKey = "SuperSecretKeyForJwtTokenAuthorization123456789";
    private static readonly Dictionary<string, string> RefreshTokens = new();

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!Users.TryGetValue(request.Username, out var password) || password != request.Password)
            return Unauthorized();

        var accessToken = GenerateAccessToken(request.Username);
        var refreshToken = GenerateRefreshToken();

        // Store the refresh token
        RefreshTokens[refreshToken] = request.Username;

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequest request)
    {
        if (!RefreshTokens.TryGetValue(request.RefreshToken, out var username))
            return Unauthorized("Invalid refresh token.");

        // Invalidate the old refresh token
        RefreshTokens.Remove(request.RefreshToken);

        // Generate a new access token and refresh token
        var newAccessToken = GenerateAccessToken(username);
        var newRefreshToken = GenerateRefreshToken();

        // Store the new refresh token
        RefreshTokens[newRefreshToken] = username;

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
    }

    private string GenerateAccessToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddMinutes(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
}