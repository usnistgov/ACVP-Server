﻿using System.Collections.Generic;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ecc_Component
{
    public class RegisterInjections : ISupportedAlgoModeRevisions
    {
        public IEnumerable<AlgoMode> SupportedAlgoModeRevisions => new List<AlgoMode>()
        {
            AlgoMode.KAS_EccComponent_Sp800_56Ar3
        };

        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<EntropyProviderFactory>().AsImplementedInterfaces();
            builder.RegisterType<EntropyProvider>().AsImplementedInterfaces();

            builder.RegisterType<Generator<v1_0.ECC_Component.Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterChecker<v1_0.ECC_Component.Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<ValidatorAsync<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<DeferredTestCaseResolver>().AsImplementedInterfaces();

            builder.RegisterType<v1_0.ECC_Component.ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<v1_0.ECC_Component.Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactoryAsync<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory<TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<v1_0.ECC_Component.Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidatorAsync<TestGroup, TestCase>>().AsImplementedInterfaces();

            builder.RegisterType<JsonConverterProvider>().AsImplementedInterfaces();
            builder.RegisterType<ContractResolverFactory>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetSerializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
        }
    }
}
