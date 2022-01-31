using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0
{
    public class TestGroupGeneratorCounter : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "CTR";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (!parameters.PerformCounterTests)
            {
                return Task.FromResult(testGroups);
            }

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    if (direction.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        // Don't allow encrypt on key option 2
                        continue;
                    }

                    if (parameters.OverflowCounter)
                    {
                        var overflowGroup = new TestGroup
                        {
                            Direction = direction,
                            KeyingOption = keyingOption,
                            IncrementalCounter = parameters.IncrementalCounter,
                            OverflowCounter = true,
                            InternalTestType = $"overflow-{parameters.OverflowCounter.ToString()}, incremental-{parameters.IncrementalCounter.ToString()}",
                            TestType = TEST_TYPE
                        };

                        testGroups.Add(overflowGroup);
                    }

                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyingOption = keyingOption,
                        IncrementalCounter = parameters.IncrementalCounter,
                        OverflowCounter = false,
                        InternalTestType = $"overflow-{parameters.OverflowCounter.ToString()}, incremental-{parameters.IncrementalCounter.ToString()}",
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
