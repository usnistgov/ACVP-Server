using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class TestGroupGeneratorMct : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var function = ShaAttributes.GetHashFunctionFromName(parameters.Algorithm);

            var testGroup = new TestGroup
            {
                HashFunction = function,
                MessageLengths = parameters.MessageLength,
                TestType = TEST_TYPE
            };

            testGroups.Add(testGroup);
            return Task.FromResult(testGroups);
        }
    }
}
