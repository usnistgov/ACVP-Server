using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroupGeneratorKnownAnswerTests : ITestGroupGenerator<Parameters>
    {
        private const string TEST_TYPE = "KAT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var pubExpMode = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode);
            if (pubExpMode == PubExpModes.FIXED)
            {
                return testGroups;
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                var mode = RSAEnumHelpers.StringToKeyGenMode(algSpec.RandPQ);
                if (mode != KeyGenModes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    foreach (var primeTest in capability.PrimeTests)
                    {
                        var testGroup = new TestGroup
                        {
                            Mode = mode,
                            Modulo = capability.Modulo,
                            PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                            PubExp = pubExpMode,
                            TestType = TEST_TYPE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
