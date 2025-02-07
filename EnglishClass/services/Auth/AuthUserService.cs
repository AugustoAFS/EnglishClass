using EnglishClass.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnglishClass.services.Auth
{
    public class AuthUserService
    {
        private readonly IConfiguration _config;

        public AuthUserService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSecret");

            string secretKeyString = jwtSettings["SecretKey"];

            if (string.IsNullOrEmpty(secretKeyString))
            {
                throw new InvalidOperationException("A chave secreta do JWT não foi configurada corretamente.");
            }

            var secretKey = Encoding.UTF8.GetBytes(secretKeyString);

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()), // Apenas o ID
                new Claim("username", user.Username) ,// Apenas o nome de usuário
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };


            var key = new SymmetricSecurityKey(secretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
