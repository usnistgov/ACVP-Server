using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NIST.CVP.Generation.SHA1;

namespace SHA1
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static Action<ContainerBuilder> OverrideRegistrations;

        public static IContainer Container { get { return _container; } }

        public static void IoCConfiguration()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Generator>();
            builder.RegisterType<NIST.CVP.Generation.SHA1.SHA1>().AsImplementedInterfaces();
            builder.RegisterType<SHA1_MCT>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
            builder.RegisterType<SHAFactory>().AsImplementedInterfaces();
            builder.RegisterType<MCTTestGroupFactory>().AsImplementedInterfaces();

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}
