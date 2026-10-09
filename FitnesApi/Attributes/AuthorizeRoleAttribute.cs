using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FitnesApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Токен не был распознан — не авторизован
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new
                {
                    status = false,
                    message = "Требуется авторизация. Передайте токен в заголовке Authorization: Bearer {токен}"
                })
                { StatusCode = 401 };
                return;
            }

            if (_roles.Length == 0)
                return;

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == null || !_roles.Contains(userRole))
            {
                context.Result = new JsonResult(new
                {
                    status = false,
                    message = $"Доступ запрещён. Требуется роль: {string.Join(" или ", _roles)}. Ваша роль: {userRole ?? "не определена"}"
                })
                { StatusCode = 403 };
            }
        }
    }
}
