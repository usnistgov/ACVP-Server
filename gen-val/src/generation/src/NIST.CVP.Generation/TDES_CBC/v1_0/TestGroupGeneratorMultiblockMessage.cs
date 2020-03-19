using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC.v1_0
{
    public class TestGroupGeneratorMultiblockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MultiBlockMessage";

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
                        InternalTestType = TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }
            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}