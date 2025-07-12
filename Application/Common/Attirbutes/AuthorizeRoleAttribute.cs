using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Domain.Enums;
using Domain.GenericExtensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Common.Attirbutes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeRoleAttribute : AuthorizeAttribute, IAsyncActionFilter
    {
        private readonly UserRoleEnum[] _roles;

        /// <summary>
        /// Конструктор, принимающий массив ролей.
        /// </summary>
        /// <param name="userRoles"></param>
        public AuthorizeRoleAttribute(params UserRoleEnum[] userRoles)
        {
            _roles = userRoles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var roleFromToken = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                roleFromToken = roleFromToken.ToLower();
                var rolesArray = _roles
                    .Select(role => role.GetDescription<UserRoleEnum>().ToLower())
                    .ToArray();
                if (!roleFromToken.Contains(UserRoleEnum.Director.GetDescription()))
                {
                    if (string.IsNullOrEmpty(roleFromToken) || !rolesArray.Contains(roleFromToken))
                    {
                        context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                        return;
                    }
                }
            }
            catch (SecurityTokenException)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }
            catch (Exception)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
