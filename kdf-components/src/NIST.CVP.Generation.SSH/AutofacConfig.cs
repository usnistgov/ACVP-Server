using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.SSH
{
    public static class AutofacConfig
    {
        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<Ssh>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory> ().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}
