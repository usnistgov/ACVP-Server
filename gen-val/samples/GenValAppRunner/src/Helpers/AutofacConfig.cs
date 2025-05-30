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
        public static List<AlgoMode> GetSupportedAlgoModes()
        {
            var algoModes = new List<AlgoMode>();

            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] assemblies = app.GetAssemblies();
            var targetType = typeof(ISupportedAlgoModeRevisions);

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
               .Where(t => targetType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in types)
                {
                    try
                    {
                        var instance = (ISupportedAlgoModeRevisions)Activator.CreateInstance(type);
                        if (instance?.SupportedAlgoModeRevisions != null)
                        {
                            algoModes.AddRange(instance.SupportedAlgoModeRevisions);
                        }
                    }
                    catch (Exception ex)
                    {
                            Console.Error.WriteLine($"[GetSupportedAlgoModes] Failed to create instance of {type.FullName}: {ex.Message}");
                    }
                }
            }

            return algoModes.Distinct().ToList();
        }
        public static List<AlgoModeInfo> GetSupportedAlgoModeInfos()
        {
            var modes = GetSupportedAlgoModes();
            var result = new List<AlgoModeInfo>();

            foreach (var mode in modes)
             {
                  var memInfo = typeof(AlgoMode).GetMember(mode.ToString()).FirstOrDefault();
                  var enumAttr = memInfo?.GetCustomAttribute<EnumMemberAttribute>();
                  var value = enumAttr?.Value ?? mode.ToString();

                 // Attempt to split on the last dash before version
                  var lastDash = value.LastIndexOf('-');
                   if (lastDash >= 0 && lastDash < value.Length - 1)
                     {
                         var cipher = value.Substring(0, lastDash);
                         var version = value.Substring(lastDash + 1);

                         result.Add(new AlgoModeInfo
                       {
                          Cipher = cipher,
                          Version = version
                       });
                     }
                  else
                    {
                        // Fallback if splitting fails
                      result.Add(new AlgoModeInfo
                        {
                          Cipher = value,
                          Version = "unknown"
                        });
                    }
             }

                return result;
        }

        
    }


}