using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf.ContractResolvers
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
