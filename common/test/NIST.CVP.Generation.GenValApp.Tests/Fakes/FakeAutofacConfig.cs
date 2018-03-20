using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace NIST.CVP.Generation.GenValApp.Tests.Fakes
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

            builder.RegisterType<FakeGenerator>().AsImplementedInterfaces();
            builder.RegisterType<FakeValidator>().AsImplementedInterfaces();

            _container = builder.Build();
        }
    }
}
