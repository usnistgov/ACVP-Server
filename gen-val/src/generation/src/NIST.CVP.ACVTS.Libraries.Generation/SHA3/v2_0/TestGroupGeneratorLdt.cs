using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class TestGroupGeneratorLdt : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "LDT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var function = ShaAttributes.GetHashFunctionFromName(parameters.Algorithm);

            if (parameters.PerformLargeDataTest.Any())
            {
                var testGroup = new TestGroup
                {
                    LargeDataSizes = parameters.PerformLargeDataTest,
                    HashFunction = function,
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
