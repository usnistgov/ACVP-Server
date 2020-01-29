using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Services;

namespace NIST.CVP.Common.Helpers
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
        private const string SETTINGS_FILE = "sharedappsettings";
        private const string SETTINGS_EXTENSION = "json";

        /// <summary>
        /// Bootstraps application from configuration file directory.
        ///
        /// Returns a <see cref="IServiceProvider"/> that has registered many of the reused
        /// components within the ACVP application.
        /// </summary>
        /// <param name="configurationFileDirectory">The directory configuration files are located.</param>
        /// <returns></returns>
        public static IServiceProvider Bootstrap(string configurationFileDirectory)
        {
            IConfigurationRoot configuration = GetConfigurationRoot(configurationFileDirectory);

            return GetBaseServiceCollection(configuration).BuildServiceProvider();
        }

        /// <summary>
        /// Returns a service provider given the provided service collection
        /// </summary>
        /// <param name="serviceCollection">Collection of services to be registered with IOC container.</param>
        /// <returns></returns>
        public static IServiceProvider Bootstrap(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Gets a configuration root from the configuration file directory.
        /// This <see cref="IConfigurationRoot"/> can be used for building a <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="configurationFileDirectory">The location of the configuration json files</param>
        /// <returns></returns>
        public static IConfigurationRoot GetConfigurationRoot(string configurationFileDirectory)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
            {
                /* TODO this could fall back to an environment,
                 * when/if driver is updated to check for var
                 */
                throw new Exception("ASPNETCORE_ENVIRONMENT env variable not set.");
            }

            Console.WriteLine($"Bootstrapping application using environment {env}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{configurationFileDirectory}{SETTINGS_FILE}.{SETTINGS_EXTENSION}", optional: false, reloadOnChange: false)
                .AddJsonFile($"{configurationFileDirectory}{SETTINGS_FILE}.{env}.{SETTINGS_EXTENSION}", optional: false, reloadOnChange: false)
                .AddJsonFile($"{configurationFileDirectory}appsettings.{SETTINGS_EXTENSION}", optional: true, reloadOnChange: false)
                .AddJsonFile($"{configurationFileDirectory}appsettings.{env}.{SETTINGS_EXTENSION}", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        }

        /// <summary>
        /// Gets the initial <see cref="IServiceCollection"/> with environment configurations
        /// based on the provided <see cref="IConfigurationRoot"/>.
        ///
        /// The returned <see cref="IServiceCollection"/> is additive, so the consumer can add
        /// additional registrations prior to the <see cref="IServiceCollection"/> be built into a
        /// <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="configurationRoot">The configuration root <see cref="GetConfigurationRoot"/></param>
        /// <returns>An additive <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection GetBaseServiceCollection(IConfigurationRoot configurationRoot)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();

            serviceCollection.Configure<IConfiguration>(configurationRoot);
            serviceCollection.AddSingleton<IConfiguration>(configurationRoot);

            serviceCollection.AddSingleton<IDbConnectionStringFactory, DbConnectionStringFactory>();
            serviceCollection.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

            serviceCollection.Configure<EnvironmentConfig>(configurationRoot.GetSection(nameof(EnvironmentConfig)));
            serviceCollection.Configure<PoolConfig>(configurationRoot.GetSection(nameof(PoolConfig)));
            serviceCollection.Configure<OrleansConfig>(configurationRoot.GetSection(nameof(OrleansConfig)));

            return serviceCollection;
        }

        /// <summary>
        /// Additional IOC injections provided via configuration files.
        /// </summary>
        /// <remarks>
        /// This should be refactored at some point We're currently using two separate IOC containers when
        /// the dotnet core one seems more than sufficient. Dotnet core's IOC container seems
        /// to have a fair amount of control to it
        /// TODO replace autofac with dotnet core?
        /// </remarks>
        /// <param name="serviceProvider">The .net core IOC provider for the instance</param>
        /// <param name="builder">The builder that will create the <see cref="IContainer"/></param>
        public static void RegisterConfigurationInjections(IServiceProvider serviceProvider, ContainerBuilder builder)
        {
            builder.Register(context => serviceProvider.GetService<IConfiguration>());
            builder.Register(context => serviceProvider.GetService<IDbConnectionStringFactory>());

            builder.Register(context => serviceProvider.GetService<IOptions<EnvironmentConfig>>());
            builder.Register(context => serviceProvider.GetService<IOptions<PoolConfig>>());
            builder.Register(context => serviceProvider.GetService<IOptions<OrleansConfig>>());
        }
    }
}