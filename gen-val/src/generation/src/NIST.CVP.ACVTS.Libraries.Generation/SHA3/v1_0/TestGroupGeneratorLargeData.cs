using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestGroupGeneratorLargeData : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "LDT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var function = ShaAttributes.GetHashFunctionFromName(parameters.Algorithm);

            // Only for SHA3 for now
            if (function.Mode == ModeValues.SHA3 && parameters.PerformLargeDataTest.Any())
            {
                foreach (var digSize in parameters.DigestSizes)
                {
                    var testGroup = new TestGroup
                    {
                        LargeDataSizes = parameters.PerformLargeDataTest,
                        Function = function.Mode,
                        DigestSize = function.DigestSize,
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
