using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorPartialBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "partialblock";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.PayloadLen.ContainsValueOtherThan(128))
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
                            PayloadLength = parameters.PayloadLen,

                            InternalTestType = LABEL
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }
            
            return testGroups;
        }
    }
}
