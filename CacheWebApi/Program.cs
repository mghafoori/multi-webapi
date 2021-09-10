using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace cache_webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            var host = hostBuilder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            
            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseUrls("https://localhost:6001;http://localhost:6000;")
                .UseStartup<Startup>();
            });

            hostBuilder.ConfigureHostConfiguration(configHost => {
                configHost.SetBasePath(Directory.GetCurrentDirectory());
                configHost.AddJsonFile("test.json", optional: true);
            });

            return hostBuilder;
        }
    }
}
