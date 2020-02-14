using System.Security.Cryptography.X509Certificates;
using CVP.DatabaseInterface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Public.Providers;

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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(configureOptions =>
                        {
                            configureOptions.ServerCertificate = new X509Certificate2("/Users/ctc/Documents/postman-certs/cceli-localhost.p12", "test");
                            configureOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        });
                    });
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
                    services.AddTransient<ITotpProvider, TotpProvider>();
                });
    }
}
