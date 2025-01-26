using Claims.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Claims.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "EntityNotFoundException encountered. Entity ID: {entityId}", ex.EntityId);
                await PrepareErrorResponse(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (DomainValidationException ex)
            {
                _logger.LogWarning(ex, "DomainException encountered.");
                await PrepareErrorResponse(context, ex.Message, HttpStatusCode.BadRequest);
            }
        }

        private static async Task PrepareErrorResponse(HttpContext context, string message, HttpStatusCode responseCode)
        {
            var response = CreateErrorResponseMessage(message);
            context.Response.StatusCode = (int)responseCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
        }

        private static string CreateErrorResponseMessage(string errorMessage)
        {
            var error = new ResponseError(errorMessage);
            var response = JsonSerializer.Serialize(error);
            return response;
        }

        public class ResponseError
        {
            public string Message { get; }

            public ResponseError(string message)
            {
                Message = message;
            }
        }
    }
}
