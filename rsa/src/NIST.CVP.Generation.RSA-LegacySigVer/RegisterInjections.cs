using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.RSA_SigVer;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_LegacySigVer
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<KeyBuilder>().AsImplementedInterfaces();
            builder.RegisterType<KeyComposerFactory>().AsImplementedInterfaces();
            builder.RegisterType<PrimeGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<SignatureBuilder>().AsImplementedInterfaces();
            builder.RegisterType<PaddingFactory>().AsImplementedInterfaces();
            builder.RegisterType<ShaFactory>().AsImplementedInterfaces();

            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();

            builder.RegisterType<Validator<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();
        }
    }
}
