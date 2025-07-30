using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ultimate.Exceptions.ExceptionTypes;
using Ultimate.Exceptions.Models;

namespace Ultimate.Exceptions.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Continue request pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            if (exception is UltimateUnAuthorizedException unAuthorizedException)
            {
                await WriteProblemAsync(httpContext, StatusCodes.Status401Unauthorized, "UnAuthorized", unAuthorizedException.Message);
            }
            else if (exception is UltimateForbiddenException forbiddenException)
            {
                await WriteProblemAsync(httpContext, StatusCodes.Status403Forbidden, "Forbidden", forbiddenException.Message);
            }
            else if (exception is UltimateValidationException or UltimateUnProcessableEntityException)
            {
                string method = httpContext.Request.Method.ToUpperInvariant();
                int statusCode = (method == HttpMethods.Post || method == HttpMethods.Put)
                    ? StatusCodes.Status422UnprocessableEntity
                    : StatusCodes.Status400BadRequest;

                string title = statusCode == StatusCodes.Status422UnprocessableEntity
                    ? "Unprocessable Entity"
                    : "Bad Request";

                List<ErrorDetail>? errorDetails = exception switch
                {
                    UltimateValidationException ve => ve.ErrorDetails,
                    UltimateUnProcessableEntityException ue => ue.ErrorDetails,
                    _ => null
                };

                var problemDetails = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title
                };
                problemDetails.Extensions.Add("invalidParams", errorDetails);

                httpContext.Response.StatusCode = statusCode;
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
            }
            else if (exception is UltimateNotFoundException notFoundException)
            {
                await WriteProblemAsync(httpContext, StatusCodes.Status404NotFound, "Not Found", $"{notFoundException.Entity} - Not Found");
            }
            else if (exception is UltimateConflictException conflictException)
            {
                await WriteProblemAsync(httpContext, StatusCodes.Status409Conflict, "Conflict", $"{conflictException.Entity} - Duplicate");
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception occurred");

                string problemTitle = exception.Message ?? "";
                string problemDetail = exception.StackTrace ?? "";

                bool isProd = _environment.EnvironmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);
                if (isProd)
                {
                    problemTitle = "Internal Server Error";
                    problemDetail = "Internal Server Error Occurred!";
                }

                await WriteProblemAsync(httpContext, StatusCodes.Status500InternalServerError, problemTitle, problemDetail);
            }
        }

        private async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string? detail = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
