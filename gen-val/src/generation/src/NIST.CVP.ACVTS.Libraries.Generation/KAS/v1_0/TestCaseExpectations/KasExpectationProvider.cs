using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.TestCaseExpectations
{
    public class KasExpectationProvider<TTestGroup, TTestCase, TKasAlgoAttributes> : TestCaseExpectationProviderBase<KasValTestDisposition>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasAlgoAttributes>
        where TKasAlgoAttributes : IKasAlgoAttributes
    {
        public KasExpectationProvider(TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes> testGroup)
        {
            var validityTestCaseOptions = new List<KasValTestDisposition>();
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
            if ((testGroup.Function.HasFlag(KasAssurance.FullVal)) && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceIutStaticPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                if (testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
                {
                    validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
                }

                if (testGroup.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair)
                {
                    validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerEphemeralPublicKey, numberOfScenariosPerType);
                }
            }

            if (testGroup.Function.HasFlag(KasAssurance.PartialVal) && testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
            }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestsForValidityGroups - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(KasValTestDisposition.Success, numberOfSuccessTests);

            LoadExpectationReasons(validityTestCaseOptions);
        }
    }
}
