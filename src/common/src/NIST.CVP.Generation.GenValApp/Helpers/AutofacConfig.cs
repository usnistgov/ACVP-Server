using System;
using Autofac;
using NIST.CVP.Crypto.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.GenValApp.Models;


namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer GetContainer()
        {
            return _container;
        }

        public static void IoCConfiguration(IServiceProvider serviceProvider, string algorithm, string mode, string revision, string dllLocation)
        {
            var builder = new ContainerBuilder();
            EntryPointConfigHelper.RegisterConfigurationInjections(serviceProvider, builder);

            var algoMode = AlgoModeLookupHelper.GetAlgoModeFromStrings(algorithm, mode, revision);

            // TODO this shouldn't be done here, fix with nuget maybe?
            // Crypto and Oracle Registration
            var crypto = new Crypto.RegisterInjections();
            crypto.RegisterTypes(builder, algoMode);
            var oracle = new Crypto.Oracle.RegisterInjections();
            oracle.RegisterTypes(builder, algoMode);
            
            var iocRegisterables = GenValResolver.ResolveIocInjectables(
                serviceProvider.GetService<IOptions<AlgorithmConfig>>().Value, 
                algorithm, 
                mode, 
                revision,
                dllLocation
            );

            foreach (var iocRegisterable in iocRegisterables)
            {
                iocRegisterable.RegisterTypes(builder, algoMode);
            }
            
            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
    
}