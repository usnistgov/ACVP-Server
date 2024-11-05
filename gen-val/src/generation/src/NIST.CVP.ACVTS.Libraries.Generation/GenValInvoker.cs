using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using Serilog;
using Serilog.Context;

namespace NIST.CVP.ACVTS.Libraries.Generation
{
    public class GenValInvoker : IGenValInvoker
    {
        private readonly IServiceProvider _serviceProvider;

        public GenValInvoker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ParameterCheckResponse CheckParameters(ParameterCheckRequest request)
        {
            var algoMode = DetermineAlgoModeFromRegistration(request);
            var containerBuilder = GetContainerBuilder();

            using var container = GetContainer(algoMode, containerBuilder).BeginLifetimeScope();
            var parameterChecker = container.Resolve<IParameterChecker>();
            return parameterChecker.CheckParameters(request);
        }

        public async Task<GenerateResponse> GenerateAsync(GenerateRequest request, long vsId)
        {
            using (LogContext.PushProperty("VsID", vsId))
            using (LogContext.PushProperty("Application", "Generator"))
            {
                var algoMode = DetermineAlgoModeFromRegistration(request);
                var containerBuilder = GetContainerBuilder();
                RegisterGenValSingletons(algoMode, containerBuilder);

                Log.Information($"Running Generation for algo: {EnumHelpers.GetEnumDescriptionFromEnum(algoMode)}");

                using var container = GetContainer(algoMode, containerBuilder).BeginLifetimeScope();
                var generator = container.Resolve<IGenerator>();
                var generatorTask = generator.GenerateAsync(request);
                return await generatorTask;
            }
        }

        public async Task<ValidateResponse> ValidateAsync(ValidateRequest request, long vsId)
        {
            using (LogContext.PushProperty("VsID", vsId))
            using (LogContext.PushProperty("Application", "Validator"))
            {
                var algoMode = DetermineAlgoModeFromValidationRequest(request);
                var containerBuilder = GetContainerBuilder();
                RegisterGenValSingletons(algoMode, containerBuilder);

                Log.Information($"Running Validation for algo: {EnumHelpers.GetEnumDescriptionFromEnum(algoMode)}");

                using var container = GetContainer(algoMode, containerBuilder).BeginLifetimeScope();
                var validator = container.Resolve<IValidator>();
                return await validator.ValidateAsync(request);
            }
        }

        private AlgoMode DetermineAlgoModeFromRegistration(ParameterCheckRequest request)
        {
            return DetermineAlgoModeFromRegistration(request.RegistrationJson);
        }

        private AlgoMode DetermineAlgoModeFromRegistration(GenerateRequest request)
        {
            return DetermineAlgoModeFromRegistration(request.RegistrationJson);
        }

        private AlgoMode DetermineAlgoModeFromRegistration(string registration)
        {
            var parameters = JsonConvert.DeserializeObject<ParametersBase>(registration);
            return AlgoModeLookupHelper.GetAlgoModeFromStrings(parameters.Algorithm, parameters.Mode, parameters.Revision);
        }

        private AlgoMode DetermineAlgoModeFromValidationRequest(ValidateRequest request)
        {
            var internalProjection = JsonConvert.DeserializeObject<TestVectorSetBase>(request.InternalJson);
            return AlgoModeLookupHelper.GetAlgoModeFromStrings(internalProjection.Algorithm, internalProjection.Mode, internalProjection.Revision);
        }

        private ContainerBuilder GetContainerBuilder()
        {
            return new ContainerBuilder();
        }

        private void RegisterGenValSingletons(AlgoMode algoMode, ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(context => _serviceProvider.GetRequiredService<IClusterClientFactory>());
            containerBuilder.Register(context => _serviceProvider.GetRequiredService<IRandom800_90>());
            containerBuilder.Register(context => _serviceProvider.GetRequiredService<IOracle>());
        }

        private IContainer GetContainer(AlgoMode algoMode, ContainerBuilder containerBuilder)
        {
            EntryPointConfigHelper.RegisterConfigurationInjections(_serviceProvider, containerBuilder);

            RegisterGenVals(containerBuilder, algoMode);

            return containerBuilder.Build();
        }

        /// <summary>
        /// Register the GenVals for the specified <see cref="AlgoMode"/>.
        /// </summary>
        /// <param name="builder">The IOC builder - note this is modified by the method.</param>
        /// <param name="algoMode">The algoMode to register.</param>
        /// <returns></returns>
        private static void RegisterGenVals(ContainerBuilder builder, AlgoMode algoMode)
        {
            var genVals = GetAlgoModeRevisionInjectables(algoMode);

            genVals.RegisterTypes(builder, algoMode);
        }

        /// <summary>
        /// Get the correct set of IOC classes registered, based on the algoMode
        /// </summary>
        /// <param name="algoMode">The algomode to use for determining the <see cref="ISupportedAlgoModeRevisions"/> to use.</param>
        /// <returns></returns>
        public static ISupportedAlgoModeRevisions GetAlgoModeRevisionInjectables(AlgoMode algoMode)
        {
            var candidateAlgoModeRevisions = GetSupportedAlgoModeRevisions();

            return candidateAlgoModeRevisions.First(w => w.SupportedAlgoModeRevisions.Contains(algoMode));
        }

        /// <summary>
        /// Returns the list of candidate <see cref="ISupportedAlgoModeRevisions"/>s - set of genvals per algo/mode/revision.
        /// </summary>
        /// <returns>list of classes that can support IOC registration for an algorithm.</returns>
        public static List<ISupportedAlgoModeRevisions> GetSupportedAlgoModeRevisions()
        {
            var types = new List<ISupportedAlgoModeRevisions>();

            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] assembly = app.GetAssemblies();
            var targetType = typeof(ISupportedAlgoModeRevisions);

            foreach (var a in assembly.Where(x => x.FullName.Contains("NIST.CVP.ACVTS.Libraries.Generation")))
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
