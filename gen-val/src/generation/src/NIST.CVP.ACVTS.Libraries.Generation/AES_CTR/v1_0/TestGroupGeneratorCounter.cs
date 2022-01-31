using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestGroupGeneratorCounter : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "CTR";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // Skip building these groups is the option is disabled
            if (!parameters.PerformCounterTests)
            {
                return Task.FromResult(testGroups);
            }

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

            return Task.FromResult(testGroups);
        }
    }
}
