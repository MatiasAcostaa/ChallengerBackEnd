using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Infrastructure.Security.Interfaces;

namespace WebAPI.Infrastructure.Security;

public class TokenHandlerService : ITokenHandlerService
{
    private readonly Jwt _jwt;
    
    public TokenHandlerService(IOptionsMonitor<Jwt> optionsMonitor)
    {
        _jwt = optionsMonitor.CurrentValue;
    }
    
    public string GenerateJwtToken(ITokenParameters parameters)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwt.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", parameters.Id),
                new Claim(JwtRegisteredClaimNames.Sub, parameters.UserName),
                new Claim(JwtRegisteredClaimNames.Email, parameters.UserName)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }
}