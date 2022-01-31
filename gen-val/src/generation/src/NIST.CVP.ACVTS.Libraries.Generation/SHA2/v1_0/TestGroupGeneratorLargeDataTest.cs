using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class TestGroupGeneratorLargeDataTest : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "LDT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.PerformLargeDataTest.Any())
            {
                foreach (var digSize in parameters.DigestSizes)
                {
                    var testGroup = new TestGroup
                    {
                        LargeDataSizes = parameters.PerformLargeDataTest,
                        Function = ShaAttributes.StringToMode(parameters.Algorithm),
                        DigestSize = ShaAttributes.StringToDigest(digSize),
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
