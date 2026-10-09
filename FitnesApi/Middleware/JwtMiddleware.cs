using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnesApi.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring(7);
                AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
                var handler = new JwtSecurityTokenHandler();

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                context.User = principal;
            }
            catch
            {
                // Токен невалиден — просто не устанавливаем пользователя
            }
        }
    }
}
