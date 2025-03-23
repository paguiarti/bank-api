using BankAPI.Application.Dtos;
using System.Text.Json;

namespace BankAPI.API.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            CustomApiResponse<object> response;
            int statusCode;
            
            response = CustomApiResponse<object>.FailResponse("Ocorreu um erro interno no servidor.", StatusCodes.Status500InternalServerError);
            statusCode = StatusCodes.Status500InternalServerError;

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
