using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Travel.Core.Interfaces;

namespace Travel.Core.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute(IJwtService jwtService, string[] roles) : Attribute, IAuthorizationFilter
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly string[] _roles = roles;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = _jwtService.GetUserRoles(token);
            if (userRoles == null || !_roles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult("You do not have permission to access this resource.");
            }   
        }
    }
}
