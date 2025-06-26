using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string _TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var result = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(parameters.Algorithm);

            var testGroups = new[] {
                new TestGroup
                {
                    ShaMode = result.shaMode,
                    TestType = _TEST_TYPE,
                    ShaDigestSize = result.shaDigestSize,
                    KeyLen = parameters.KeyLen,
                    MacLen = parameters.MacLen,
                    MessageLen = parameters.MessageLen
                }
            };

            return Task.FromResult(testGroups.ToList());
        }
    }
}
