using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroupGeneratorPartialBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "partialblock";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.DataLength.ContainsValueOtherThan(128))
            {
                foreach (var direction in parameters.Direction)
                {
                    foreach (var keyLength in parameters.KeyLen)
                    {
                        var testGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyLength = keyLength,

                            // Only test case generator that cares about this information
                            DataLength = parameters.DataLength,

                            TestType = LABEL
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }
            
            return testGroups;
        }
    }
}
