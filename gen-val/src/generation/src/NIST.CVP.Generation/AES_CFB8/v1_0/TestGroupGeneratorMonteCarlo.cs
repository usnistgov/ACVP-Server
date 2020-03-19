using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB8.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup()
                    {
                        AlgoMode = algoMode,
                        Function = direction,
                        KeyLength = keyLength,
                        TestType = _MCT_TEST_TYPE_LABEL,
                        InternalTestType = _MCT_TEST_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
