using System;
using System.Net;
using System.Threading.Tasks;
using Cache.WebApi.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Cache.WebApi.Middlewares
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
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<CustomCultureAttribute>();
            if (attribute != null)
            {
                var culture = httpContext.Request.Query["culture"];
                Console.WriteLine($"Culture: {culture}");
                httpContext.Response.Headers.Add("culture-code", culture);
                if(!attribute.IsAuthorized(culture))
                {
                    httpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsync($"Culture not supported: {culture}");
                    return;
                }                
            }
            await _next(httpContext);
        }
    }
}
