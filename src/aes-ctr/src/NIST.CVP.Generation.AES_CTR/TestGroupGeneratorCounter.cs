using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroupGeneratorCounter : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "CTR";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    if (parameters.OverflowCounter)
                    {
                        var overflowGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyLength = keyLength,
                            IncrementalCounter = parameters.IncrementalCounter,
                            OverflowCounter = true,
                            TestType = LABEL,
                            InternalTestType = LABEL
                        };

                        testGroups.Add(overflowGroup);
                    }

                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyLength = keyLength,
                        IncrementalCounter = parameters.IncrementalCounter,
                        OverflowCounter = false,
                        TestType = LABEL,
                        InternalTestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
