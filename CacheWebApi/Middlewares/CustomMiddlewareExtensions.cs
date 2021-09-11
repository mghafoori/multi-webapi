using Microsoft.AspNetCore.Builder;

namespace Cache.WebApi.Middlewares
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomHeaderMiddleware>();
        }

        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }


}
