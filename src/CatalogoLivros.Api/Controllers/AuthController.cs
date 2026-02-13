using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CatalogoLivros.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult GerarToken([FromBody] GerarTokenRequest? request)
    {
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer nao configurado.");
        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience nao configurado.");
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key nao configurado.");

        var nome = string.IsNullOrWhiteSpace(request?.Nome) ? "usuario-teste" : request.Nome.Trim();
        var role = string.IsNullOrWhiteSpace(request?.Role) ? "admin" : request.Role.Trim();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, nome),
            new(JwtRegisteredClaimNames.Name, nome),
            new(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new
        {
            accessToken = jwt,
            tokenType = "Bearer",
            expiresIn = 3600
        });
    }
}

public sealed class GerarTokenRequest
{
    public string? Nome { get; init; }
    public string? Role { get; init; }
}
