using Countries.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Countries.Api.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            return builder;
        }
    }
}
