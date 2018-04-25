using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.KeyWrap.TDES.ContractResolvers
{
    public class ContractResolverFactory : IContractResolverFactory<TestGroup, TestCase>
    {
        public ProjectionContractResolverBase<TestGroup, TestCase> GetContractResolver(Projection projection)
        {
            switch (projection)
            {
                case Projection.Server:
                    return new ServerProjectionContractResolver<TestGroup, TestCase>();
                case Projection.Prompt:
                    return new PromptProjectionContractResolver();
                case Projection.Result:
                    return new ResultProjectionContractResolver();
                default:
                    throw new ArgumentException($"Invalid {nameof(projection)} ({projection})");
            }
        }
    }
}
