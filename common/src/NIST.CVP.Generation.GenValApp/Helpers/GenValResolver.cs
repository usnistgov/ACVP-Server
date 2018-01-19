using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class GenValResolver
    {
        public static Dictionary<(string Algorithm, string Mode), (string genVals, HashSet<string> additionalDependencies)> Map = 
            new Dictionary<(string Algorithm, string Mode), (string genVals, HashSet<string> additionalDependencies)>
        {
            {
                ("aes", "ecb"),
                ("NIST.CVP.Generation.AES_ECB.dll", new HashSet<string>()
                {
                    "NIST.CVP.Crypto.AES.dll",
                    "NIST.CVP.Crypto.AES_ECB.dll",
                })
            }
        };

        public static IRegisterInjections ResolveIocInjectables(string algorithm, string mode, string dllLocation)
        {
            var iTypeToDiscover = typeof(IRegisterInjections);

            if (!Map.TryFirst(t => 
                t.Key.Algorithm.Equals(algorithm, StringComparison.OrdinalIgnoreCase) 
                && t.Key.Mode.Equals(mode, StringComparison.OrdinalIgnoreCase),
                out var mappingResult)
            )
            {
                throw new ArgumentException($"Unable to find dll mapping for {algorithm} and {mode}");
            }

            var genValDll = mappingResult.Value.genVals;
            var fullgenValDllPath = $@"{dllLocation}{genValDll}";
            if (!File.Exists(fullgenValDllPath))
            {
                throw new ArgumentException($"Unable to locate mapped dll {fullgenValDllPath}");
            }

            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullgenValDllPath);
            var concreteType = assembly.GetTypes().Single(x => iTypeToDiscover.IsAssignableFrom(x));
            var concrete = (IRegisterInjections)Activator.CreateInstance(concreteType);

            // Load additional dependant assemblies
            if (mappingResult.Value.additionalDependencies != null)
            {
                foreach (var assemblyLocation in mappingResult.Value.additionalDependencies)
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath($@"{dllLocation}{assemblyLocation}");
                }
            }

            return concrete;
        }
    }
}