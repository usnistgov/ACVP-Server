using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using System.Runtime.Serialization;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Generation;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Helpers
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

        var oracle = new RegisterInjections();
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
        var interfaceType = typeof(ISupportedAlgoModeRevisions);
        var results = new List<ISupportedAlgoModeRevisions>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray();

                foreach (var e in ex.LoaderExceptions)
                {
                    Console.Error.WriteLine($"[TypeLoad Error] {e?.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Assembly Error] Failed to read types from {assembly.FullName}: {ex.Message}");
                continue;
            }

            foreach (var type in types)
            {
                if (type == null || !interfaceType.IsAssignableFrom(type) || type.IsInterface || type.IsAbstract)
                    continue;

                try
                {
                    if (Activator.CreateInstance(type) is ISupportedAlgoModeRevisions instance)
                        results.Add(instance);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[Activator Error] {type.FullName}: {ex.Message}");
                }
            }
        }

        return results;
    }

    public static List<AlgoMode> GetSupportedAlgoModes()
    {
        return GetSupportedAlgoModeRevisions()
            .SelectMany(x => x.SupportedAlgoModeRevisions ?? Array.Empty<AlgoMode>())
            .Distinct()
            .ToList();
    }

    public static List<AlgoModeInfo> GetSupportedAlgoModeInfos()
    {
        var result = new List<AlgoModeInfo>();

        foreach (var mode in GetSupportedAlgoModes())
        {
            var memberInfo = typeof(AlgoMode).GetMember(mode.ToString()).FirstOrDefault();
            var attr = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
            var value = attr?.Value ?? mode.ToString();

            // Try to split on last dash (e.g., "AES-GCM-1_0" -> "AES-GCM", "1_0")
            var lastDash = value.LastIndexOf('-');
            if (lastDash >= 0 && lastDash < value.Length - 1)
            {
                var cipher = value.Substring(0, lastDash);
                var version = value.Substring(lastDash + 1);
                result.Add(new AlgoModeInfo { Cipher = cipher, Version = version });
            }
            else
            {
                result.Add(new AlgoModeInfo { Cipher = value, Version = "unknown" });
            }
        }

        return result;
    }
}

}