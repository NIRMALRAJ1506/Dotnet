using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReservationAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private const string SecretKey = "MyNameisNirmalMyNameisNirmalMyNameisNirmalMyNameisNirmal"; // Use a secure key in production
    private readonly string _issuer = "dotnetclient";
    private readonly string _audience = "your_audience";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginViewModel model)
    {
        if (model.UserName == "nirmal" && model.Password == "nirmalcena")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, model.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        return Unauthorized();
    }
}
