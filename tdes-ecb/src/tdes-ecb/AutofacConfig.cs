using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Generation.TDES_ECB;
using NIST.CVP.Math;

namespace tdes_ecb
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


            builder.RegisterType<Generator>();
           
            builder.RegisterType<TdesEcb>().AsImplementedInterfaces();
            builder.RegisterType<TDES_ECB_MCT>().AsImplementedInterfaces();
            builder.RegisterType<MonteCarloKeyMaker>().AsImplementedInterfaces();
            builder.RegisterType<KnownAnswerTestFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
          
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();

            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser<Parameters>>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();
       

            OverrideRegistrations?.Invoke(builder);

            _container = builder.Build();

        }
    }
}
