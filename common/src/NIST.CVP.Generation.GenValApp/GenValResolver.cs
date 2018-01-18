using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.GenValApp
{
    public static class GenValResolver
    {
        public static Dictionary<(Algorithm Algorithm, string Mode), (string genVals, HashSet<string> additionalDependencies)> Map = 
            new Dictionary<(Algorithm Algorithm, string Mode), (string genVals, HashSet<string> additionalDependencies)>
        {
            {
                (Algorithm.AesEcb, string.Empty),
                ("NIST.CVP.Generation.AES_ECB.dll", new HashSet<string>()
                {
                    "NIST.CVP.Crypto.AES.dll"
                })
            }
        };

        public static IRegisterInjections ResolveIocInjectables(Algorithm algorithm, string mode, string dllLocation)
        {
            var iTypeToDiscover = typeof(IRegisterInjections);

            if (!Map.TryFirst(t => 
                t.Key.Algorithm == algorithm 
                && t.Key.Mode == mode, 
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
            foreach (var assemblyLocation in mappingResult.Value.additionalDependencies)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath($@"{dllLocation}{assemblyLocation}");
            }

            return concrete;
        }
    }
}