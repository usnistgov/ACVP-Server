using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
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
                            PubExp = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode),
                            FixedPubExp = new BitString(parameters.FixedPubExp),
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
