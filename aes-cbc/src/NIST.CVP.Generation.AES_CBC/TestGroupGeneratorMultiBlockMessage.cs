using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestGroupGeneratorMultiBlockMessage : ITestGroupGenerator<Parameters>
    {
        public const string MMT_TYPE_LABEL = "MMT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Function = function,
                        KeyLength = keyLength,
                        TestType = MMT_TYPE_LABEL
                    };
                    testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
