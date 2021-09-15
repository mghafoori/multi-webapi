using System;
using System.Linq;
using Cache.WebApi;
using Cache.WebApi.Middlewares;
using CacheWebApi.Models;
using CacheWebApi.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cache_webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            foreach (var item in configuration.AsEnumerable().OrderBy(c => c.Key))
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Create JWT tokens here: http://jwtbuilder.jamiekurtz.com
            byte[] keyBytes = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(config =>
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "Distributed Dev Organization",
                        ValidAudience = "www.humber.ca",
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    }
                );
            services.AddScoped<IValidator<CacheItemModel>, CacheItemModelValidator>();
            services.AddSingleton<ICacheStore, CacheStore>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "cache_webapi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "cache_webapi v1"));
            }

            app.UseHttpsRedirection();

            app.UseCustomHeader();

            app.UseRouting();

            app.UseRequestCulture();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
