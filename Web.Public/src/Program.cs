using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Public.Configs;

namespace Web.Public
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var executingLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var rootDirectory = Path.GetDirectoryName(executingLocation) + Path.DirectorySeparatorChar;
            CreateHostBuilder(args, rootDirectory).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.IsNullOrWhiteSpace(env))
                    {
                        throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
                    }

                    context.HostingEnvironment.EnvironmentName = env;

                    builder
                        //.AddJsonFile($"{directoryConfig}sharedappsettings.json", false, false)
                        //.AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", false, false)
                        .AddJsonFile($"{directoryConfig}appsettings.json", false, false);
                        //.AddJsonFile($"{directoryConfig}appsettings.{env}.json", false, false);
                })
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
                    services.RegisterAcvpAdminServices();
                    services.Configure<TotpConfig>(hostContext.Configuration.GetSection("Totp"));
                    services.Configure<AlgorithmConfig>(hostContext.Configuration.GetSection("Algorithm"));
                });
    }
}
