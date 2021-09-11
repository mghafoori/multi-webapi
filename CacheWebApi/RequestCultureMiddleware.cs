using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cache_webapi
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var culture = httpContext.Request.Query["culture"];            
            Console.WriteLine($"Culture: {culture}");
            httpContext.Response.Headers.Add("culture-code", culture);
            // Call the next delegate/middleware in the pipeline
            await _next(httpContext);
        }
    }
}
