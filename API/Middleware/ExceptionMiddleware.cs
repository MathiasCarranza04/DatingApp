using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;

        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); //intento ejecutar el middelware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); //registro el error
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //establezco codigo de error 500

                var response = _env.IsDevelopment()
                 ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) // el ? indica que estamos en development mode
                 : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error"); // los : indican que no estamos en dev mode

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //serializo el objecto ApiException en formato json

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json); //doy la respuesta en formato json

            }
        }

    }
}