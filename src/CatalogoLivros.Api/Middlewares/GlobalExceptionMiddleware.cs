using System.Text.Json;

namespace CatalogoLivros.Api.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            var traceId = context.TraceIdentifier;
            _logger.LogError(ex, "Erro nao tratado. TraceId: {TraceId}", traceId);

            var (statusCode, title) = Mapear(ex);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var body = new
            {
                type = "about:blank",
                title,
                status = statusCode,
                traceId
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }

    private static (int statusCode, string title) Mapear(Exception ex)
    {
        return ex switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, ex.Message),
            KeyNotFoundException => (StatusCodes.Status404NotFound, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor.")
        };
    }
}
