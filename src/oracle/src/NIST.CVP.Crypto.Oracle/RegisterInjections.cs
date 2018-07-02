using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Common;

namespace NIST.CVP.Crypto.Oracle
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            builder.RegisterType<Oracle>().AsImplementedInterfaces();
        }
    }
}
