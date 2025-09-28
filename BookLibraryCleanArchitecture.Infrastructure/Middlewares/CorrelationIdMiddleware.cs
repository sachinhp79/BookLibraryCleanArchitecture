using BookLibraryCleanArchitecture.Common.Constants;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace BookLibraryCleanArchitecture.Client.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderKey = MiddlewareConstants.CORRELATION_ID_HEADER;
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 1️⃣ Try to get Correlation ID from incoming request header
            var correlationId = context.Request.Headers.ContainsKey(HeaderKey)
                ? context.Request.Headers[HeaderKey].ToString()
                : Guid.NewGuid().ToString(); // Generate new if missing

            // Add to response header so client can see it
            context.Response.Headers[HeaderKey] = correlationId;

            // 4️⃣ Push into Serilog context so every log line includes it
            LogContext.PushProperty(MiddlewareConstants.CORRELATION_ID, correlationId);

            // 5️⃣ Store in HttpContext.Items for downstream access
            context.Items[MiddlewareConstants.CORRELATION_ID] = correlationId;

            await _next(context); // Continue pipeline
        }
    }

}
