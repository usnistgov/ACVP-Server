using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var curveParam in parameters.Curve)
            {
                var testGroup = new TestGroup
                {
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveParam),
                };

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
