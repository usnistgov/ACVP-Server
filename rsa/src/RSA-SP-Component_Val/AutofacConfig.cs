using System;
using Autofac;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.RSA_SPComponent;
using NIST.CVP.Math;

namespace RSA_SP_Component_Val
{
public class AutofacConfig
{
    private static IContainer _container;
    public static Action<ContainerBuilder> OverrideRegistrations;
    public static IContainer Container => _container;

    public static void IoCConfiguration()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<KeyBuilder>().AsImplementedInterfaces();
        builder.RegisterType<KeyComposerFactory>().AsImplementedInterfaces();

        builder.RegisterType<Generator<Parameters, TestVectorSet, TestGroup, TestCase>>().AsImplementedInterfaces();
        builder.RegisterType<Validator<TestVectorSet, TestCase>>().AsImplementedInterfaces();
        builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
        builder.RegisterType<TestReconstitutor>().AsImplementedInterfaces();
        builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
        builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
        builder.RegisterType<ResultValidator<TestCase>>().AsImplementedInterfaces();
        builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
        builder.RegisterType<DynamicParser>().AsImplementedInterfaces();
        builder.RegisterType<Random800_90>().AsImplementedInterfaces();
        builder.RegisterType<TestCaseValidatorFactory>().AsImplementedInterfaces();

        OverrideRegistrations?.Invoke(builder);
        _container = builder.Build();
    }
}
}
