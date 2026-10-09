using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnesApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        // Таблица ролей (соответствует id_role в БД)
        public static readonly Dictionary<int, string> RoleNames = new()
        {
            { 1, "Admin" },
            { 2, "Trainer" },
            { 3, "Client" },
            { 4, "Organizer" }
        };

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int id_user, string email, int id_role)
        {
            var roleName = RoleNames.TryGetValue(id_role, out var name) ? name : "Client";

            var claims = new[]
            {
                new Claim("id_user", id_user.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
