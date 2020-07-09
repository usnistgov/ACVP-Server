using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using JwtCreatinator.Models;
using JwtCreatinator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NIST.CVP.Common.Helpers;
using Web.Public;
using Web.Public.Configs;

namespace JwtCreatinator
{
	/// <summary>
	/// This application takes in a json with test session ids and client cert subjects, and generates new JWTs for them.
	/// </summary>
	class Program
	{
		private const string SampleJson = @"
[
	{
		""testSessionId"": 1,
		""clientCertSubject"": ""E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US""
	},
	{
		""testSessionId"": 2,
		""clientCertSubject"": ""E=russell.hammett@nist.gov, CN=Russ Hammett, OU=ACVTS, O=NIST, L=Gaithersburg, S=Maryland, C=US""
	}
]
";
		
		static void Main(string[] args)
		{
			var serviceProvider = Bootstrap();
			var logger = serviceProvider.GetService<ILogger<Program>>();
			
			var jwtCreatinator = serviceProvider.GetService<IJwtCreatinator>();

			try
			{
				Console.WriteLine("This utility can be used to create new JWTs based on a test session and client cert subject.");
				Console.WriteLine("The format of the file is:");
				Console.WriteLine(SampleJson);
				
				Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}-----{Environment.NewLine}{Environment.NewLine}");
				
				Console.WriteLine($"Enter the full filepath to a json file in the format shown above.{Environment.NewLine}");
				
				var filename = Console.ReadLine().Replace(@"""", string.Empty);
				
				Console.WriteLine(Environment.NewLine);

				if (!File.Exists(filename))
				{
					throw new Exception("Unable to find file {filename}.  Exiting.");
				}

				var requests = JsonSerializer.Deserialize<List<JwtRenewRequest>>(File.ReadAllText(filename));

				if (requests == null || requests.Count == 0)
				{
					throw new Exception($"Unable to parse any valid requests from {filename}");
				}
				
				logger.LogInformation($"Attempting to create new JWTs for {requests.Count} test sessions.");
				
				var newJwts = jwtCreatinator.CreateJwts(requests);
				var newJwtsJson = JsonSerializer.Serialize(newJwts, new JsonSerializerOptions()
				{
					WriteIndented = true
				});
				
				logger.LogInformation($"Processed {newJwts.Count} requests.");
				logger.LogInformation(newJwtsJson);

				var newFilename = $"{Path.GetDirectoryName(filename)}{Path.DirectorySeparatorChar}{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_newJwts.json";
				File.WriteAllText(newFilename, newJwtsJson);
				
				logger.LogInformation($"New jwts saved to: {newFilename}");
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
				.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
				.AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: false);

			var configuration = builder.Build();

			var serviceCollection = new ServiceCollection();

			serviceCollection.AddLogging(logging => logging.AddConsole());
			serviceCollection.AddSingleton<IConfiguration>(configuration);
			serviceCollection.Configure<JwtConfig>(configuration.GetSection("Jwt"));
			serviceCollection.Configure<TotpConfig>(configuration.GetSection("Totp"));
			serviceCollection.Configure<AlgorithmConfig>(configuration.GetSection("Algorithm"));
			serviceCollection.Configure<VectorSetConfig>(configuration.GetSection("VectorSet"));
			serviceCollection.Configure<TestSessionConfig>(configuration.GetSection("TestSession"));
			
			serviceCollection.RegisterAcvpPublicServices();
			
			serviceCollection.AddSingleton<IJwtCreatinator, Services.JwtCreatinator>();
			
			var serviceProvider = serviceCollection.BuildServiceProvider();
			return serviceProvider;
		}
	}
}