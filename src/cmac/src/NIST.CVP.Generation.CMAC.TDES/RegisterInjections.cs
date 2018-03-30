using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<CmacFactory>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelInternals>().AsImplementedInterfaces();
            builder.RegisterType<RijndaelFactory>().AsImplementedInterfaces();

            builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory<TestCaseGeneratorGen, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseValidatorFactory<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestReconstitutor<TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
            builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
        }
    }
}