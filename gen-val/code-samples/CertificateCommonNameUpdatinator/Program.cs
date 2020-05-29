using System;
using System.IO;
using CertificateCommonNameUpdatinator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NLog.Config;

namespace CertificateCommonNameUpdatinator
{
	/// <summary>
	/// This application is used to take in all the acvp user cert data,
	/// extract the common name from the cert, and save that common name
	/// back to the table.  The current common names on the table are
	/// partially obfuscated, and will not longer be after this tool is run.
	/// </summary>
	class Program
	{
		static void Main(string[] args)
		{
			var serviceProvider = Bootstrap();
			var logger = serviceProvider.GetService<ILogger<Program>>();
			
			var userSubjectService = serviceProvider.GetService<IAcvpUserSubjectService>();

			try
			{
				userSubjectService.UpdateUserSubjectsFromCertBytes();
			}
			catch (Exception e)
			{
				logger.LogError(e, e.Message);
			}
			
			serviceProvider.Dispose();
			Console.WriteLine("Press any key to close.");
			Console.ReadKey();
		}

		private static ServiceProvider Bootstrap()
		{
			var directoryConfig = EntryPointConfigHelper.GetRootDirectory();
			
			string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			if (string.IsNullOrWhiteSpace(env))
			{
				// TODO this could fall back to an environment, rather than exceptioning?
				throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
			}

			Console.WriteLine($"Bootstrapping application using environment {env}");

			var builder = new ConfigurationBuilder()
				.SetBasePath(directoryConfig)
				.AddJsonFile($"sharedappsettings.json", optional: false, reloadOnChange: false)
				.AddJsonFile($"sharedappsettings.{env}.json", optional: false, reloadOnChange: false);

			var configuration = builder.Build();

			var serviceCollection = new ServiceCollection();

			serviceCollection.AddLogging(logging => logging.AddConsole());
			serviceCollection.AddSingleton<IConfiguration>(configuration);
			serviceCollection.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
			serviceCollection.AddSingleton<IAcvpUserProvider, AcvpUserProvider>();
			serviceCollection.AddSingleton<IAcvpUserService, AcvpUserService>();
			serviceCollection.AddSingleton<IAcvpUserSubjectService, AcvpUserSubjectService>();

			var serviceProvider = serviceCollection.BuildServiceProvider();
			return serviceProvider;
		}
	}
}