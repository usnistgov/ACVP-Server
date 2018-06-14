using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.KMAC.ContractResolvers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KMAC
{
    public class RegisterInjections : IRegisterInjections
    {
        public void RegisterTypes(ContainerBuilder builder, AlgoMode algoMode)
        {
            throw new System.NotImplementedException();
        }
    }
}
