using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using sltlang.Adapters.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using SLThree.Extensions;
using System.Text;

namespace sltlang
{
    public class SwaggerAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _routePrefix;

        public SwaggerAuthorizationMiddleware(RequestDelegate next, string routePrefix)
        {
            _next = next;
            _routePrefix = routePrefix.TrimEnd('/');
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_routePrefix, out var remaining) && remaining.StartsWithSegments("/index.html"))
            {
                var auth = context.User.HasPermission(Common.AuthService.Enums.Permission.RootPermission) || context.User.HasPermission(Common.AuthService.Enums.Permission.Swagger);

                if (!auth)
                {
                    context.Response.StatusCode = 403;
                    return;
                }
            }

            await _next(context);
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Проверяем наличие атрибута [Authorize] на контроллере или методе
            var hasAuthorizeAttribute =
                (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any())
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorizeAttribute)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    }
                };
            }
        }
    }
}