using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0.ContractResolvers
{
    public class ContractResolverFactory : IContractResolverFactory<TestGroup, TestCase>
    {
        public ProjectionContractResolverBase<TestGroup, TestCase> GetContractResolver(Projection projection)
        {
            return projection switch
            {
                Projection.Server => new ServerProjectionContractResolver<TestGroup, TestCase>(),
                Projection.Prompt => new PromptProjectionContractResolver(),
                Projection.Result => new ResultProjectionContractResolver(),
                _ => throw new ArgumentException($"Invalid {nameof(projection)} ({projection})")
            };
        }
    }
}
