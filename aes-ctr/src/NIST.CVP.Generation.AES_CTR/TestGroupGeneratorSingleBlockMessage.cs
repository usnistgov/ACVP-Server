using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "singleblock";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyLength = keyLength,

                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
