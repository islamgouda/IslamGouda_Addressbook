using api.Errors;
using System.Net;
using System.Text.Json;

namespace api.Middelware
{
    public class ExceptionMiddelware//:IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddelware> _logger;
        private readonly IHostEnvironment _environment;
        public ExceptionMiddelware(RequestDelegate requestDelegate, ILogger<ExceptionMiddelware> logger, IHostEnvironment environment)
        {
            this._next = requestDelegate;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)//, RequestDelegate next)
        {
            try {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                var Response = _environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);
                var option =new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(Response, option);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
