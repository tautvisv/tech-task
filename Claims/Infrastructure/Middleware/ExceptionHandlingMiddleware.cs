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
                _logger.LogInformation(ex, "EntityNotFoundException encountered. Entity ID: {entityId}", ex.EntityId);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                var response = CreateErrorResponseMessage(ex.Message);
                await context.Response.WriteAsync(response);
            }
            catch (DomainException ex)
            {
                _logger.LogInformation(ex, "DomainException encountered.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var response = CreateErrorResponseMessage(ex.Message);
                await context.Response.WriteAsync(response);
            }
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
