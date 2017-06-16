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

            if (!parameters.KeyGenModes.Contains("B.3.3", StringComparer.OrdinalIgnoreCase))
            {
                return testGroups;
            }

            if (parameters.PubExpMode.ToLower() != "random")
            {
                return testGroups;
            }

            foreach (var modulo in parameters.Moduli)
            {
                foreach (var primeTest in parameters.PrimeTests)
                {
                    var testGroup = new TestGroup
                    {
                        Mode = KeyGenModes.B33,
                        Modulo = modulo,
                        PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                        PubExp = PubExpModes.RANDOM,
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
