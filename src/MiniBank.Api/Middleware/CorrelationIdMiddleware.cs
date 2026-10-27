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
            string? correlationId = context.Request.Headers[HeaderName];
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
            }
            context.Response.Headers[HeaderName] = correlationId;

            using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
            {
                await _next(context);
            }
        }
    }
}
