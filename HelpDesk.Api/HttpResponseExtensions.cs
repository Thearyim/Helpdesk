using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HelpDesk.Api
{
    public static class HttpResponseExtensions
    {
        public static void AddCorsHeaders(this HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", new StringValues("*"));
            response.Headers.Add("Access-Control-Allow-Headers", new StringValues("*"));
            response.Headers.Add("Access-Control-Allow-Methods", new StringValues("GET, POST, PUT, DELETE"));
        }

        public static IActionResult InternalServerError(this ControllerBase controller, object value)
        {
            return new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        public static IActionResult Unauthorized(this ControllerBase controller, object value)
        {
            return new ObjectResult(value)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        public static IActionResult UserLoginInvalid(this ControllerBase controller)
        {
            return controller.Unauthorized("Username or password is invalid.");
        }
    }
}
