using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctionalTest : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = ShaAttributes.StringToMode(parameters.Algorithm),
                    DigestSize = ShaAttributes.StringToDigest(digestSize),
                    MessageLength = parameters.MessageLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };
                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
