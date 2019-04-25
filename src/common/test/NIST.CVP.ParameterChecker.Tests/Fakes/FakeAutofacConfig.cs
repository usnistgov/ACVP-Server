using System;
using Autofac;

namespace NIST.CVP.ParameterChecker.Tests.Fakes
{
    public class FakeAutofacConfig
    {
        private IContainer _container;

        public IContainer GetContainer()
        {
            IoCConfiguration();
            return _container;
        }

        private void IoCConfiguration()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FakeParameterChecker>().AsImplementedInterfaces();

            _container = builder.Build();
        }
    }
}
