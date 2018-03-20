using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class GenValResolver
    {
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        public static IRegisterInjections ResolveIocInjectables(AlgorithmConfig algorithmConfig, string algorithm, string mode, string dllLocation)
        {
            var iTypeToDiscover = typeof(IRegisterInjections);

            if (!algorithmConfig.Algorithms.TryFirst(t => 
                t.Algorithm.Equals(algorithm, StringComparison.OrdinalIgnoreCase) 
                && t.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase),
                out var mappingResult)
            )
            {
                string errorMsg =
                    $"Unable to find dll mapping for {nameof(algorithm)} ({algorithm}) and {nameof(mode)} ({mode})";
                Logger.Error(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            var genValDll = mappingResult.EntryDll;
            var fullgenValDllPath = $@"{dllLocation}{genValDll}";
            if (!File.Exists(fullgenValDllPath))
            {
                string errorMsg =
                    $"Unable to locate mapped dll {fullgenValDllPath}";
                Logger.Error(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            IRegisterInjections concrete = null;
            try
            {
                /*
                 Note: loading additional assemblies first,
                 because (example) if "main" generation Gen.CMAC.AES.dll
                 depends on Gen.CMAC.dll, which has not yet been loaded.

                 Gen.CMAC.dll is an additional assembly, and should be loaded first.
                */
                // Load additional dependant assemblies
                if (mappingResult.AdditionalDependencies != null)
                {
                    foreach (var additionalDependency in mappingResult.AdditionalDependencies)
                    {
                        AssemblyLoadContext.Default.LoadFromAssemblyPath(
                            $@"{dllLocation}{additionalDependency.DependencyDll}"
                        );
                    }
                }

                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullgenValDllPath);
                var concreteType = assembly.GetTypes().Single(x => iTypeToDiscover.IsAssignableFrom(x));
                concrete = (IRegisterInjections) Activator.CreateInstance(concreteType);
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Logger.Error(loaderException.Message);
                }

                throw;
            }

            return concrete;
        }
    }
}