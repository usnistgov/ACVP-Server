using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Generation.KAS.v1_0.Helpers
{
    public static class TestCaseDispositionHelper
    {
        public static List<KasValTestDisposition> PopulateValidityTestCaseOptions<TTestGroup, TTestCase, TKasDsaAlgoAttributes>(
            TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> testGroup
        )
            where TKasDsaAlgoAttributes : IKasAlgoAttributes
            where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
            where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
        {
            List<KasValTestDisposition> validityTestCaseOptions = new List<KasValTestDisposition>();
            const int numberOfTestsForValidityGroups = 25;
            const int numberOfScenariosPerType = 2;
            
            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedOi, numberOfScenariosPerType);
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedDkm, numberOfScenariosPerType);
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedMacData, numberOfScenariosPerType);
                validityTestCaseOptions.Add(KasValTestDisposition.SuccessLeadingZeroNibbleDkm, numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash tag)
            validityTestCaseOptions.Add(KasValTestDisposition.FailChangedZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(KasValTestDisposition.SuccessLeadingZeroNibbleZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(KasValTestDisposition.FailChangedTag, numberOfScenariosPerType);

            // Conditions based on assurances
            if ((testGroup.Function.HasFlag(KasAssurance.KeyPairGen) || testGroup.Function.HasFlag(KasAssurance.FullVal)) 
                && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceIutStaticPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                if (testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
                {
                    validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey,
                        numberOfScenariosPerType);
                }

                if (testGroup.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair)
                {
                    validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerEphemeralPublicKey,
                        numberOfScenariosPerType);
                }
            }

            if (testGroup.Function.HasFlag(KasAssurance.PartialVal) 
                && testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
            }

            if ((testGroup.KasMode == KasMode.KdfKc || testGroup.Function.HasFlag(KasAssurance.KeyPairGen)
                || testGroup.Function.HasFlag(KasAssurance.KeyRegen)) 
                && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceIutStaticPrivateKey, numberOfScenariosPerType);
            }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestsForValidityGroups - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(KasValTestDisposition.Success, numberOfSuccessTests);

            // Reorder the test case conditions randomly
            validityTestCaseOptions = validityTestCaseOptions.OrderBy(ob => Guid.NewGuid()).ToList();

            return validityTestCaseOptions;
        }

        public static KasValTestDisposition GetTestCaseIntention(List<KasValTestDisposition> validityTestCaseOptions)
        {
            var nextDisposition = validityTestCaseOptions[0];
            validityTestCaseOptions.RemoveAt(0);

            return nextDisposition;
        }
    }
}