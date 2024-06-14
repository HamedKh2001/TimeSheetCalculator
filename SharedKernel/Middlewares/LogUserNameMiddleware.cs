using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System.Threading.Tasks;

namespace SharedKernel.Middlewares
{
    public class LogUserNameMiddleware
    {
        private readonly RequestDelegate next;

        public LogUserNameMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LogContext.PushProperty("remote_user", context.User.Identity.Name);
            LogContext.PushProperty("remote_addr", context.Connection.RemoteIpAddress);
            LogContext.PushProperty("RemotePort", context.Connection.RemotePort);
            LogContext.PushProperty("status", context.Response.StatusCode);
            LogContext.PushProperty("query_string", context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null);
            LogContext.PushProperty("server_addr", context.Connection.LocalIpAddress);
            LogContext.PushProperty("LocalPort", context.Connection.LocalPort);
            LogContext.PushProperty("server_protocol", context.Request.Protocol);
            LogContext.PushProperty("uri", context.Request.GetDisplayUrl());
            LogContext.PushProperty("request_method", context.Request.Method);
            LogContext.PushProperty("hostname", context.Request.Host.HasValue ? context.Request.Host.Value : null);
            if (context.Request.Headers.TryGetValue("User-Agent", out var userAgent))
                LogContext.PushProperty("http_user_agent", userAgent.ToString());
            LogContext.PushProperty("bytes_sent", context.Request.ContentLength.HasValue ? context.Request.ContentLength.Value : null);
            LogContext.PushProperty("bytes_received", context.Response.ContentLength.HasValue ? context.Response.ContentLength.Value : null);
            //LogContext.PushProperty("bytes_received", context.Request.Headers[]);

            return next(context);
        }
    }
}
