namespace OfferService.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        
        // Log request
        _logger.LogInformation(
            "Incoming Request: {Method} {Path} from {RemoteIP}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");

        // Call the next middleware
        await _next(context);

        // Log response
        var duration = DateTime.UtcNow - startTime;
        _logger.LogInformation(
            "Outgoing Response: {Method} {Path} responded {StatusCode} in {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}