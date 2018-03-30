using System;
using Autofac;
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

        public static void IoCConfiguration(AlgorithmConfig algorithmConfig, string algorithm, string mode, string dllLocation)
        {
            var builder = new ContainerBuilder();

            var algoMode = AlgoModeLookupHelper.GetAlgoModeFromStrings(algorithm, mode);

            var genValInjectables = GenValResolver.ResolveIocInjectables(algorithmConfig, algorithm, mode, dllLocation);
            genValInjectables.RegisterTypes(builder, algoMode);

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
    
}