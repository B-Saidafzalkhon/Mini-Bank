namespace MiniBank.Api.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "MiniBank-Correlation-Id";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(token))
            {
                token = "default";
            }
            context.Response.Headers[HeaderName] = token;

            context.Response.Headers["X-Server-Name"] = "MiniBank";

            using (_logger.BeginScope("UserId: {UserId}", token))
            {
                await _next(context);
            }
        }
    }
}
