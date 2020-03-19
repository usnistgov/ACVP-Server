using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorCounter : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "CTR";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
