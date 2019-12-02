using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC.v1_0
{
    public class TestGroupGeneratorMultiBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string MMT_TYPE_LABEL = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
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
                        TestType = MMT_TYPE_LABEL,
                        InternalTestType = "MMT"
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
