using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Stecpoint.ServiceB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            
            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration)
                .MinimumLevel.Information()
                //  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning) 
                //  .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning) 
                .WriteTo.Console(outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .Enrich.FromLogContext()  
                .Enrich.WithProperty("AppName", "ServiceA")
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .CreateLogger();

            IWebHostBuilder hostBuilder =
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseUrls("http://*:4301");

            hostBuilder.Build().Run();
        }
    }
}