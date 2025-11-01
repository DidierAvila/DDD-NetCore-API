using System.Net;
using System.Text.Json;

namespace Platform.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response already started; skipping custom error response.");
                return;
            }

            var statusCode = GetStatusCode(exception);
            var code = GetErrorCode(exception);
            var traceId = context.TraceIdentifier;
            var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var cid)
                ? cid.ToString()
                : null;

            _logger.LogError(
                exception,
                "Unhandled exception. Path: {Path}, StatusCode: {StatusCode}, TraceId: {TraceId}, CorrelationId: {CorrelationId}",
                context.Request.Path,
                (int)statusCode,
                traceId,
                correlationId);

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            var payload = new
            {
                success = false,
                status = (int)statusCode,
                code,
                message = exception.Message,
                path = context.Request.Path.Value,
                traceId,
                correlationId,
                timestamp = DateTimeOffset.UtcNow
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload, options));
        }

        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                InvalidOperationException => HttpStatusCode.BadRequest,
                ArgumentNullException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                System.Security.SecurityException => HttpStatusCode.Forbidden,
                Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException => HttpStatusCode.Conflict,
                Microsoft.EntityFrameworkCore.DbUpdateException => HttpStatusCode.Conflict,
                TimeoutException => HttpStatusCode.GatewayTimeout,
                NotImplementedException => HttpStatusCode.NotImplemented,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private static string GetErrorCode(Exception exception)
        {
            return exception switch
            {
                KeyNotFoundException => "not_found",
                InvalidOperationException => "invalid_operation",
                ArgumentNullException => "invalid_argument",
                ArgumentException => "invalid_argument",
                UnauthorizedAccessException => "unauthorized",
                System.Security.SecurityException => "forbidden",
                Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException => "concurrency_conflict",
                Microsoft.EntityFrameworkCore.DbUpdateException => "db_update_error",
                TimeoutException => "timeout",
                NotImplementedException => "not_implemented",
                _ => "internal_error"
            };
        }
    }
}