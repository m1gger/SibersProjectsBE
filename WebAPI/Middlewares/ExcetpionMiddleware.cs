using System.Net;
using FluentValidation;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status;
            object response;

            switch (exception)
            {
                case ValidationException validationException:
                    status = HttpStatusCode.BadRequest;
                    response = new
                    {
                        errors = validationException.Errors.Select(e => new
                        {
                            property = e?.PropertyName,
                            message = e?.ErrorMessage,
                            code = e?.ErrorCode,
                            StateId = e?.CustomState
                        })
                    };
                    break;

                case ArgumentException or InvalidOperationException:
                    status = HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = exception.Message
                    };
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    response = new
                    {
                        error = "Internal Server Error"
                    };
                    break;
            }

            context.Response.StatusCode = (int)status;
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
