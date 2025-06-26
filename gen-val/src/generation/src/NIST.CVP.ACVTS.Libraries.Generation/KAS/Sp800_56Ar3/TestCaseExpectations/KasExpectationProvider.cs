using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.TestCaseExpectations
{
    public class KasExpectationProvider<TTestGroup, TTestCase, TKeyPair> : TestCaseExpectationProviderBase<KasValTestDisposition>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        public KasExpectationProvider(TestGroupBase<TTestGroup, TTestCase, TKeyPair> testGroup, bool includeFailureTests)
        {
            const int numberOfScenariosPerType = 2;
            var validityTestCaseOptions = new List<KasValTestDisposition>();

            var numberOfTestCases = includeFailureTests ? 25 : 10;

            if (!includeFailureTests)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.Success, numberOfTestCases);
                LoadExpectationReasons(validityTestCaseOptions);
                return;
            }

            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedDkm, numberOfScenariosPerType);
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedMacData, numberOfScenariosPerType);
                validityTestCaseOptions.Add(KasValTestDisposition.SuccessLeadingZeroNibbleDkm, 1);
            }

            if (testGroup.KasMode == KasMode.KdfKc)
            {
                validityTestCaseOptions.Add(KasValTestDisposition.FailChangedTag, numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash)
            validityTestCaseOptions.Add(KasValTestDisposition.FailChangedZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(KasValTestDisposition.SuccessLeadingZeroNibbleZ, 1);

            // // Conditions based on assurances
            // if ((testGroup.Function.HasFlag(KasAssurance.KeyPairGen) || testGroup.Function.HasFlag(KasAssurance.FullVal)) 
            //     && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            // {
            //     validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceIutStaticPublicKey, numberOfScenariosPerType);
            // }
            //
            // if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            // {
            //     if (testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
            //     {
            //         validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey,
            //             numberOfScenariosPerType);
            //     }
            //
            //     if (testGroup.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair)
            //     {
            //         validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerEphemeralPublicKey,
            //             numberOfScenariosPerType);
            //     }
            // }
            //
            // if (testGroup.Function.HasFlag(KasAssurance.PartialVal) 
            //     && testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
            // {
            //     validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
            // }
            //
            // if ((testGroup.KasMode == KasMode.KdfKc || testGroup.Function.HasFlag(KasAssurance.KeyPairGen)
            //     || testGroup.Function.HasFlag(KasAssurance.KeyRegen)) 
            //     && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            // {
            //     validityTestCaseOptions.Add(KasValTestDisposition.FailAssuranceIutStaticPrivateKey, numberOfScenariosPerType);
            // }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestCases - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(KasValTestDisposition.Success, numberOfSuccessTests);

            LoadExpectationReasons(validityTestCaseOptions);
        }
    }
}
