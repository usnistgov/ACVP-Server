using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyVer.TestCaseExpectations;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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

            return Task.FromResult(testGroups);
        }
    }
}
