using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string _MCT_TEST_TYPE_LABEL = "MCT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup()
                    {
                        Function = direction,
                        KeyLength = keyLength,
                        TestType = _MCT_TEST_TYPE_LABEL,
                        InternalTestType = _MCT_TEST_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
