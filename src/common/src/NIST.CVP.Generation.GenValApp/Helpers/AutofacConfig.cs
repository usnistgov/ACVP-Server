using System;
using Autofac;
using NIST.CVP.Crypto.Oracle;
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

            // TODO this shouldn't be done here, fix with nuget maybe?
            // Crypto and Oracle Registration
            var crypto = new Crypto.RegisterInjections();
            crypto.RegisterTypes(builder, algoMode);
            var oracle = new NIST.CVP.Crypto.Oracle.RegisterInjections();
            oracle.RegisterTypes(builder, algoMode);

            var iocRegisterables = GenValResolver.ResolveIocInjectables(algorithmConfig, algorithm, mode, dllLocation);
            foreach (var iocRegisterable in iocRegisterables)
            {
                iocRegisterable.RegisterTypes(builder, algoMode);
            }

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
    
}