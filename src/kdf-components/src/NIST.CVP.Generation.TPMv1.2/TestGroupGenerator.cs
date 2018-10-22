using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TPMv1._2
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            return new List<TestGroup>
            {
                new TestGroup
                {
                    TestType = "AFT"
                }
            };
        }
    }
}
