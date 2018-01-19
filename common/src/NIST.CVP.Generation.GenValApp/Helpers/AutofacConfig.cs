using System;
using Autofac;

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

        public static void IoCConfiguration(string algorithm, string mode, string dllLocation)
        {
            var builder = new ContainerBuilder();

            var genValInjectables = GenValResolver.ResolveIocInjectables(algorithm, mode, dllLocation);
            genValInjectables.RegisterTypes(builder);

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }
    }
    
}