using BookLibraryCleanArchitecture.Client.Middlewares;

namespace BookLibraryCleanArchitecture.Client.Extensions
{

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBookLibraryMiddlewares(this IApplicationBuilder app)
        {
            // Global exception handler — catches all errors
            //app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Correlation ID — enriches logs with request ID
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseExceptionHandler();

            // 🔐 Optional: Serilog request logging (if enabled)
            // app.UseSerilogRequestLogging();

            return app;
        }
    }
}