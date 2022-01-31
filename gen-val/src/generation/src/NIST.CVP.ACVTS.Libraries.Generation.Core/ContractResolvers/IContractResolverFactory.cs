using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers
{
    public interface IContractResolverFactory<TTestGroup, TTestCase>
        where TTestGroup : class, ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        ProjectionContractResolverBase<TTestGroup, TTestCase> GetContractResolver(Projection projection);
    }
}
