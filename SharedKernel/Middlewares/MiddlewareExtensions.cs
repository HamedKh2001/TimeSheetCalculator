using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace SharedKernel.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseElasticMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<LogUserNameMiddleware>();

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var uris = configuration.GetSection("CrossSettings:AllowedOrigins").Get<string[]>();
            builder.UseCors(options => options.WithOrigins(uris).AllowAnyMethod().AllowCredentials().AllowAnyHeader().AllowAnyMethod().Build());
            return builder;
        }
    }
}
