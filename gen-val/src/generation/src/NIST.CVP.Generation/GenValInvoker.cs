using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using Serilog;
using Serilog.Context;

namespace NIST.CVP.Generation
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
            using var container = GetContainer(algoMode).BeginLifetimeScope();
            var parameterChecker = container.Resolve<IParameterChecker>();
            return parameterChecker.CheckParameters(request);
        }

        public GenerateResponse Generate(GenerateRequest request, int vsId)
        {
            var algoMode = DetermineAlgoModeFromRegistration(request);
            using var container = GetContainer(algoMode).BeginLifetimeScope();
            var generator = container.Resolve<IGenerator>();
            
            using (LogContext.PushProperty("VsID", vsId))
            using (LogContext.PushProperty("Application", "Generator"))
            {
                Log.Information($"Running Generation for algo: {EnumHelpers.GetEnumDescriptionFromEnum(algoMode)}");
                return generator.Generate(request);   
            }
        }
        
        public ValidateResponse Validate(ValidateRequest request, int vsId)
        {
            var algoMode = DetermineAlgoModeFromValidationRequest(request);
            using var container = GetContainer(algoMode).BeginLifetimeScope();
            var validator = container.Resolve<IValidator>();
            
            using (LogContext.PushProperty("VsID", vsId))
            using (LogContext.PushProperty("Application", "Validator"))
            {
                Log.Information($"Running Validation for algo: {EnumHelpers.GetEnumDescriptionFromEnum(algoMode)}");
                return validator.Validate(request);
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

        private IContainer GetContainer(AlgoMode algoMode)
        {
            var builder = new ContainerBuilder();
            EntryPointConfigHelper.RegisterConfigurationInjections(_serviceProvider, builder);
            
            var oracle = new Crypto.Oracle.RegisterInjections();
            oracle.RegisterTypes(builder, algoMode);

            RegisterGenVals(builder, algoMode);

            return builder.Build();
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

            return candidateAlgoModeRevisions.FirstOrDefault(w => w.SupportedAlgoModeRevisions.Contains(algoMode));
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