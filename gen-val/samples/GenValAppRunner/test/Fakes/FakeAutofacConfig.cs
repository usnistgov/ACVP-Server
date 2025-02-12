﻿using Autofac;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Fakes
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
