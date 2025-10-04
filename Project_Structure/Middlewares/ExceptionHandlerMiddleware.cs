using Core.Exceptions;
using Core.Model;
using System.Net;

namespace Project_Structure.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private HttpContext _httpContext;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                _httpContext = httpContext;
                await _next.Invoke(httpContext);
            }
            catch(Exception ex)
            {
                await HandleException(ex);
            }
        }

        public async Task HandleException(Exception ex)
        {
            SetStatusCode(ex);
            if (_httpContext.Request.Path.ToString().Contains("/api/"))
            {
                await HandleApiException(ex);
            }
        }

        public void SetStatusCode(Exception ex)
        {
            if (ex is DataNotFoundException)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (ex is DataValidationException)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (ex is NotAuthorizedException)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (ex is NoPermissionException)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            else if (ex is ServerErrorException)
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            else
            {
                _httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        public async Task HandleApiException(Exception ex)
        {
            var error = new Dictionary<string, string>()
            {
                ["general"] = ex.Message
            };

            var jsonResponse = ApiResponse<object>.ErrorResult(error);
            await _httpContext.Response.WriteAsJsonAsync(jsonResponse);
        }
    }
}
