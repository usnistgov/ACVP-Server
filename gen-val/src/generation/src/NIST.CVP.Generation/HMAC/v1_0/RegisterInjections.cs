﻿using System.Collections.Generic;
using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.HMAC.v1_0.ContractResolvers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.HMAC.v1_0
{
    public class RegisterInjections : ISupportedAlgoModeRevisions
    {
        public IEnumerable<AlgoMode> SupportedAlgoModeRevisions => new List<AlgoMode>()
        {
            AlgoMode.HMAC_SHA1_v1_0,
            AlgoMode.HMAC_SHA2_224_v1_0,
            AlgoMode.HMAC_SHA2_256_v1_0,
            AlgoMode.HMAC_SHA2_384_v1_0,
            AlgoMode.HMAC_SHA2_512_v1_0,
            AlgoMode.HMAC_SHA2_512_224_v1_0,
            AlgoMode.HMAC_SHA2_512_256_v1_0,
            AlgoMode.HMAC_SHA3_224_v1_0,
            AlgoMode.HMAC_SHA3_256_v1_0,
            AlgoMode.HMAC_SHA3_384_v1_0,
            AlgoMode.HMAC_SHA3_512_v1_0
        };

        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterChecker<Parameters>>().AsImplementedInterfaces();

            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactoryAsync<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            builder.RegisterType<ValidatorAsync<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidatorAsync<TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();

            builder.RegisterType<JsonConverterProvider>().AsImplementedInterfaces();
            builder.RegisterType<ContractResolverFactory>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetSerializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<VectorSetDeserializer<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
        }
    }
}