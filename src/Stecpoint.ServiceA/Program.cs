using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Stecpoint.ServiceA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information) 
              //  .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning) 
                .WriteTo.Console(outputTemplate:"[{Timestamp:yyyy/MM/dd HH:mm:ss} {Level:u10}] {SourceContext:l} {Message:lj} {NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()  
                .Enrich.WithProperty("AppName", "ServiceA")
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .CreateLogger();

            IWebHostBuilder hostBuilder =
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseUrls("http://*:4300");

            hostBuilder.Build().Run();
        }
    }
}