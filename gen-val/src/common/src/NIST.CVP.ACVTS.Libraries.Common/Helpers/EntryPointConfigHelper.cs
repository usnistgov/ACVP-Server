using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;
using NIST.CVP.ACVTS.Libraries.Common.Services;

namespace NIST.CVP.ACVTS.Libraries.Common.Helpers
{
    /// <summary>
    /// Helper class for setting up configuration parsing and .net core
    /// dependency injection.
    /// </summary>
    /// <remarks>
    /// Located in "common" area as all entry points need access to this functionality -
    /// genValAppRunner, integration tests, etc.
    /// </remarks>
    public static class EntryPointConfigHelper
    {
        /// <summary>
        /// Get the root directory starting the application.
        /// </summary>
        /// <returns></returns>
        public static string GetRootDirectory()
        {
            var executingLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var rootDirectory = Path.GetDirectoryName(executingLocation) + Path.DirectorySeparatorChar;
            //Console.WriteLine($"{nameof(rootDirectory)}: {rootDirectory}");
            return rootDirectory;
        }

        /// <summary>
        /// Creates <see cref="IServiceProvider"/> using a <see cref="ConfigurationBuilder"/>.
        /// Loads sharedappsettings.json (and for all environments). as well as appsettings (and for all environments).
        ///
        /// Relies on DOTNET_ENVIRONMENT environment variable.
        /// </summary>
        /// <returns><see cref="IServiceProvider"/> with several already registered types.</returns>
        /// <exception cref="Exception">Thrown when DOTNET_ENVIRONMENT not set.</exception>
        public static IServiceProvider GetServiceProviderFromConfigurationBuilder()
        {
            string env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                env = "dev";
            }

            Console.WriteLine($"Bootstrapping application using environment {env}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"sharedappsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"sharedappsettings.{env}.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);

            var configurationRoot = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton((IConfiguration)configurationRoot);

            serviceCollection.AddHttpClient();

            serviceCollection.AddSingleton<IDbConnectionStringFactory, DbConnectionStringFactory>();
            serviceCollection.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

            serviceCollection.Configure<EnvironmentConfig>(configurationRoot.GetSection(nameof(EnvironmentConfig)));
            serviceCollection.Configure<PoolConfig>(configurationRoot.GetSection(nameof(PoolConfig)));
            serviceCollection.Configure<OrleansConfig>(configurationRoot.GetSection(nameof(OrleansConfig)));

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Additional IOC injections provided via configuration files.
        /// </summary>
        /// <remarks>
        /// This should be refactored at some point We're currently using two separate IOC containers when
        /// the dotnet core one seems more than sufficient. Dotnet core's IOC container seems
        /// to have a fair amount of control to it
        /// </remarks>
        /// <param name="serviceProvider">The .net core IOC provider for the instance</param>
        /// <param name="builder">The builder that will create the <see cref="IContainer"/></param>
        public static void RegisterConfigurationInjections(IServiceProvider serviceProvider, ContainerBuilder builder)
        {
            builder.Register(context => serviceProvider.GetRequiredService<IConfiguration>());

            builder.Register(context => serviceProvider.GetRequiredService<IDbConnectionStringFactory>());
            builder.Register(context => serviceProvider.GetRequiredService<IDbConnectionFactory>());

            builder.Register(context => serviceProvider.GetRequiredService<IOptions<EnvironmentConfig>>());
            builder.Register(context => serviceProvider.GetRequiredService<IOptions<PoolConfig>>());
            builder.Register(context => serviceProvider.GetRequiredService<IOptions<OrleansConfig>>());
        }
    }
}
