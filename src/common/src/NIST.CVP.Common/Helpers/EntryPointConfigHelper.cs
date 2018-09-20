using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

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
        private const string SETTINGS_FILE = "appsettings";
        private const string SETTINGS_EXTENSION = "json";

        public static IServiceProvider Bootstrap(string configurationFileDirectory)
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
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            
            serviceCollection.Configure<EnvironmentConfig>(configuration.GetSection(nameof(EnvironmentConfig)));
            serviceCollection.Configure<AlgorithmConfig>(configuration.GetSection(nameof(AlgorithmConfig)));
            serviceCollection.Configure<PoolConfig>(configuration.GetSection(nameof(PoolConfig)));
            serviceCollection.Configure<OrleansConfig>(configuration.GetSection(nameof(OrleansConfig)));

            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Additional IOC injections proivded via configuration files.
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
            builder.Register(context => serviceProvider.GetService<IOptions<EnvironmentConfig>>());
            builder.Register(context => serviceProvider.GetService<IOptions<AlgorithmConfig>>());
            builder.Register(context => serviceProvider.GetService<IOptions<PoolConfig>>());
            builder.Register(context => serviceProvider.GetService<IOptions<OrleansConfig>>());
        }
    }
}