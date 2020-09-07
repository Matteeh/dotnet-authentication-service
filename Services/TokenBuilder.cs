using identity.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace identity.Services
{
    public class TokenBuilder : ITokenBuilder
    {
        public string BuildToken(ApplicationUser appUser)
        {
            Claim[] claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", appUser.Email),
                new Claim("FirstName", appUser.FirstName),
                new Claim("LastName", appUser.LastName)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = Environment.GetEnvironmentVariable("AUDIENCE"),
                Issuer = Environment.GetEnvironmentVariable("ISSUER"),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}