using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_OFB.v1_0
{
    public class TestGroupGeneratorMultiBlockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "MMT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Function = function,
                        KeyLength = keyLength,
                        InternalTestType = MMT_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
