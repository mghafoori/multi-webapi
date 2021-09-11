using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cache_webapi
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;

        public CustomHeaderMiddleware(RequestDelegate nextMiddleware) 
            => _nextMiddleware = nextMiddleware;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var customHeader = httpContext.Request.Headers["custom-header"];
            if (customHeader.Any() && customHeader[0] == "DistDevRequest")
            {
                Console.WriteLine("Good request. Continuing");
                await _nextMiddleware(httpContext);
            }
            else
            {
                Console.WriteLine("Bad request. Terminating.");
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            }
        }
    }


}
