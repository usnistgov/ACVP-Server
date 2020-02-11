using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Web.Public
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration))
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHostedService<Worker>();

                    //Inject libraries
                    //services.InjectACVPCore();
                    //services.InjectACVPWorkflow();
                    //services.InjectDatabaseInterface();

                    //Inject local things
                    services.AddTransient<ITotpProvider, TotpProvider>();
                });
    }
}
