using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sup2Marché.Model.Tools
{
    public class JwtHandler
    {
        private readonly string _key;

        public JwtHandler(string key)
        {
            _key = key;
        }

        public string GenerateToken(string userId, string username)
        {
            var keyString = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", userId),
                new Claim("username", username),
            }),
                Expires = DateTime.UtcNow.AddHours(1), // Temps d'expiration du token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyString), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
