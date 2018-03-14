using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
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
