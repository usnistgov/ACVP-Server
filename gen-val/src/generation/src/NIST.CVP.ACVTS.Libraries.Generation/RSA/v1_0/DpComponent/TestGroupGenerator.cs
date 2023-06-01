using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.DpComponent
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            return Task.FromResult(new List<TestGroup>
            {
                new TestGroup
                {
                    Modulo = 2048,
                    TotalTestCases = parameters.IsSample ? 6 : 30,
                    TotalFailingCases = parameters.IsSample ? 2 : 10
                }
            });
        }
    }
}
