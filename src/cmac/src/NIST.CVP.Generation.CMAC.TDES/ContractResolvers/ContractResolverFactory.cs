using System;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.CMAC.TDES.ContractResolvers
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