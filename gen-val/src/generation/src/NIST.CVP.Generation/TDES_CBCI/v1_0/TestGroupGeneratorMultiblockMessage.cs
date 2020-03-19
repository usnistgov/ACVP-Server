using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBCI.v1_0
{
    public class TestGroupGeneratorMultiblockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        private const string INTERNAL_TEST_TYPE = "MultiBlockMessage";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    // Encrypt Keying Option 2 is not valid, skip test groups
                    if (function.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        continue;
                    }

                    var tg = new TestGroup()
                    {
                        Function = function,
                        KeyingOption = keyingOption,
                        TestType = TEST_TYPE,
                        InternalTestType = INTERNAL_TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }

    }
}
