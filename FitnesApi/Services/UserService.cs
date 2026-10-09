using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class UserService
    {
        private readonly ContextDb _context;
        private readonly JwtService _jwtService;

        public UserService(ContextDb context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.full_name) ||
                string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password) ||
                string.IsNullOrWhiteSpace(request.phone))
            {
                return new JsonResult(new { status = false, message = "Заполните все поля" });
            }

            if (request.password.Length < 4)
                return new JsonResult(new { status = false, message = "Пароль должен содержать минимум 4 символа" });

            if (request.phone.Length < 11)
                return new JsonResult(new { status = false, message = "Введите корректный номер телефона" });

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.email == request.email);

            if (existingUser != null)
                return new JsonResult(new { status = false, message = "Пользователь с такой почтой уже существует" });

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.id_role == 3);
            if (role == null)
                return new JsonResult(new { status = false, message = "Роль не найдена" });

            var user = new User
            {
                full_name = request.full_name,
                email = request.email,
                password = request.password,
                phone = request.phone,
                birth_date = request.birth_date,
                id_role = 3,
                balance = 0
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Сразу выдаём токен при регистрации
            var token = _jwtService.GenerateToken(user.id_user, user.email, user.id_role);

            return new JsonResult(new
            {
                status = true,
                message = "Регистрация выполнена успешно",
                token,
                user = new
                {
                    user.id_user,
                    user.full_name,
                    user.email,
                    user.phone,
                    user.birth_date,
                    user.balance,
                    user.id_role,
                    role_name = "Client"
                }
            });
        }

        public async Task<ActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.email) ||
                string.IsNullOrWhiteSpace(request.password))
            {
                return new JsonResult(new { status = false, message = "Введите почту и пароль" });
            }

            var user = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x =>
                    x.email == request.email &&
                    x.password == request.password);

            if (user == null)
                return new JsonResult(new { status = false, message = "Неверная почта или пароль" });

            var token = _jwtService.GenerateToken(user.id_user, user.email, user.id_role);

            return new JsonResult(new
            {
                status = true,
                message = "Авторизация успешна",
                token,
                user = new
                {
                    user.id_user,
                    user.full_name,
                    user.email,
                    user.phone,
                    user.birth_date,
                    user.balance,
                    user.id_role,
                    role_name = user.Role != null ? user.Role.name_role : ""
                }
            });
        }
    }
}
