using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            return new List<TestGroup>
            {
                new TestGroup
                {
                    Modulo = 2048,
                    TotalTestCases = parameters.IsSample ? 6 : 30,
                    TotalFailingCases = parameters.IsSample ? 2 : 10
                }
            };
        }
    }
}
