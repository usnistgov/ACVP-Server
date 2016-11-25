using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using NIST.CVP.Generation.AES_GCM;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Math;

namespace AES_GCM
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get { return _container; }
        }
        public static void IoCConfiguration()
        {
            var builder = new ContainerBuilder();


            builder.RegisterType<Generator>();
            builder.RegisterType<AES_GCMInternals>().AsImplementedInterfaces();
            builder.RegisterType<NIST.CVP.Generation.AES_GCM.AES_GCM>().AsImplementedInterfaces();
            builder.RegisterType<TestCaseGeneratorFactory>().AsImplementedInterfaces();
            builder.RegisterType<TestVectorFactory>().AsImplementedInterfaces();
            builder.RegisterType<ParameterValidator>().AsImplementedInterfaces();
            builder.RegisterType<ParameterParser>().AsImplementedInterfaces();
            builder.RegisterType<Random800_90>().AsImplementedInterfaces();


            _container = builder.Build();

        }
    }
}
