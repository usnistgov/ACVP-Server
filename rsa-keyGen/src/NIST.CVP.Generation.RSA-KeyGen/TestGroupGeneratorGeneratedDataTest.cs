using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (!parameters.KeyGenModes.Contains("B.3.3", StringComparer.OrdinalIgnoreCase))
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
                        PubExp = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode),
                        FixedPubExp = new BitString(parameters.FixedPubExp),
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
