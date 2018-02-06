using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.GenValApp.Models;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class GenValResolver
    {
        public static IRegisterInjections ResolveIocInjectables(AlgorithmConfig algorithmConfig, string algorithm, string mode, string dllLocation)
        {
            var iTypeToDiscover = typeof(IRegisterInjections);

            if (!algorithmConfig.Algorithms.TryFirst(t => 
                t.Algorithm.Equals(algorithm, StringComparison.OrdinalIgnoreCase) 
                && t.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase),
                out var mappingResult)
            )
            {
                throw new ArgumentException($"Unable to find dll mapping for {algorithm} ({algorithm}) and {mode} ({mode})");
            }

            var genValDll = mappingResult.EntryDll;
            var fullgenValDllPath = $@"{dllLocation}{genValDll}";
            if (!File.Exists(fullgenValDllPath))
            {
                throw new ArgumentException($"Unable to locate mapped dll {fullgenValDllPath}");
            }

            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullgenValDllPath);
            var concreteType = assembly.GetTypes().Single(x => iTypeToDiscover.IsAssignableFrom(x));
            var concrete = (IRegisterInjections)Activator.CreateInstance(concreteType);

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

            return concrete;
        }
    }
}