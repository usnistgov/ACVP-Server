using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_OFB;
using NIST.CVP.Generation.TDES_OFB;
using NIST.CVP.Math;
using NIST.CVP.Generation.Core;

namespace tdes_ofb
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container
        {
            get { return _container; }
        }
        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<Generator<Parameters, TestVectorSet>>().AsImplementedInterfaces();

            builder.RegisterType<TdesOfb>().AsImplementedInterfaces();
            builder.RegisterType<TDES_OFB_MCT>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorKnownAnswer>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();

            builder.RegisterType<TestVectorFactory<Parameters, TestVectorSet>>().AsImplementedInterfaces();
            builder.RegisterType<TestGroupGeneratorFactory>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();


            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}