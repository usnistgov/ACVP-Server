﻿using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static void IoCConfiguration(IServiceProvider serviceProvider, AlgoMode algoMode)
        {
            var builder = new ContainerBuilder();
            EntryPointConfigHelper.RegisterConfigurationInjections(serviceProvider, builder);

            var oracle = new Crypto.Oracle.RegisterInjections();
            oracle.RegisterTypes(builder, algoMode);

            RegisterGenVals(builder, algoMode);

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();
        }

        /// <summary>
        /// Register the GenVals for the specified <see cref="AlgoMode"/>.
        /// </summary>
        /// <param name="builder">The IOC builder</param>
        /// <param name="algoMode">The algoMode to register</param>
        /// <returns></returns>
        private static void RegisterGenVals(ContainerBuilder builder, AlgoMode algoMode)
        {
            var genVals = GetAlgoModeRevisionInjectables(algoMode);

            genVals.RegisterTypes(builder, algoMode);
        }

        public static ISupportedAlgoModeRevisions GetAlgoModeRevisionInjectables(AlgoMode algoMode)
        {
            var candidateAlgoModeRevisions = GetSupportedAlgoModeRevisions();

            return candidateAlgoModeRevisions.FirstOrDefault(w => w.SupportedAlgoModeRevisions.Contains(algoMode));
        }

        public static List<ISupportedAlgoModeRevisions> GetSupportedAlgoModeRevisions()
        {
            var types = new List<ISupportedAlgoModeRevisions>();

            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] assembly = app.GetAssemblies();
            var targetType = typeof(ISupportedAlgoModeRevisions);

            foreach (var a in assembly)
            {
                a.GetTypes()
                    .Where(w => targetType.IsAssignableFrom(w) && !w.IsInterface)
                    .ToList()
                    .ForEach(fe => types.Add((ISupportedAlgoModeRevisions)Activator.CreateInstance(fe)));
            }

            return types;
        }
    }
}