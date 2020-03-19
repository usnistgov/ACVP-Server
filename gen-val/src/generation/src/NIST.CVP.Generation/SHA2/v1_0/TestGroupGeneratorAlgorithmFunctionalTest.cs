using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctionalTest : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = SHAEnumHelpers.StringToMode(parameters.Algorithm),
                    DigestSize = SHAEnumHelpers.StringToDigest(digestSize),
                    MessageLength = parameters.MessageLength,
                    TestType = TEST_TYPE
                };
                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
