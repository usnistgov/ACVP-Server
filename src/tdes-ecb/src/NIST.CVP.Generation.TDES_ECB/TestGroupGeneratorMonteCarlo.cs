using System.Collections.Generic;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters>
    {
        private const string TEST_TYPE = "MCT";
        
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
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
                        TestType = TEST_TYPE
                    };
                    
                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }
    }
}
