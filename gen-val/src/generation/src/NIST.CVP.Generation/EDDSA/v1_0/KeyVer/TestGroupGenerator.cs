using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var curveName in parameters.Curve)
            {
                var testGroup = new TestGroup
                {
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName),
                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample)
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
