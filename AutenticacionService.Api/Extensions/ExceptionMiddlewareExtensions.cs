using AutenticacionService.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AutenticacionService.Api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AppExceptionMiddleware>();
        }
    }
}
