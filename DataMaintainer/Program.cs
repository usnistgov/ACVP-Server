using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.Email;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Serilog;

namespace DataMaintainer
{
	class Program
	{
		public static void Main(string[] args)
		{
			var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
			CreateHostBuilder(args, directoryConfig).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args, string directoryConfig) =>
			Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, builder) =>
				{
					var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
					if (string.IsNullOrWhiteSpace(env))
					{
						/* TODO this could fall back to an environment,
						 * when/if driver is updated to check for var
						 */
						throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
					}

					context.HostingEnvironment.EnvironmentName = env;

					builder
						.AddJsonFile($"{directoryConfig}sharedappsettings.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}sharedappsettings.{env}.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}appsettings.json", optional: false, reloadOnChange: false)
						.AddJsonFile($"{directoryConfig}appsettings.{env}.json", optional: false, reloadOnChange: false);
				})
				.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration))

				.ConfigureServices((hostContext, services) =>
				{
					services.Configure<EmailConfiguration>(hostContext.Configuration.GetSection("EmailConfiguration"));

					//Inject libraries
					services.InjectACVPCore();
					services.InjectDatabaseInterface();
					services.InjectMailer();

					//Inject what actually runs as the hosted service
					services.AddSingleton<IHostedService, Worker>();
				});
	}
}
