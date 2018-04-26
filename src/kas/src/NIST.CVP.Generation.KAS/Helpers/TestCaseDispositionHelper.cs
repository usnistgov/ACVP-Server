using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.KAS.Enums;

namespace NIST.CVP.Generation.KAS.Helpers
{
    public static class TestCaseDispositionHelper
    {
        public static List<TestCaseDispositionOption> PopulateValidityTestCaseOptions<TTestGroup, TTestCase, TKasDsaAlgoAttributes>(
            TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> testGroup
        )
            where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
            where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
            where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes>
        {
            List<TestCaseDispositionOption> validityTestCaseOptions = new List<TestCaseDispositionOption>();
            const int numberOfTestsForValidityGroups = 25;
            const int numberOfScenariosPerType = 2;

            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedOi, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedDkm, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedMacData, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.SuccessLeadingZeroNibbleDkm, numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash tag)
            validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(TestCaseDispositionOption.SuccessLeadingZeroNibbleZ, numberOfScenariosPerType);
            validityTestCaseOptions.Add(TestCaseDispositionOption.FailChangedTag, numberOfScenariosPerType);

            // Conditions based on assurances
            if (testGroup.Function.HasFlag(KasAssurance.KeyPairGen) || testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceIutStaticPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceServerStaticPublicKey, numberOfScenariosPerType);
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, numberOfScenariosPerType);
            }

            if (testGroup.KasMode == KasMode.KdfKc || testGroup.Function.HasFlag(KasAssurance.KeyPairGen) ||
                testGroup.Function.HasFlag(KasAssurance.KeyRegen))
            {
                validityTestCaseOptions.Add(TestCaseDispositionOption.FailAssuranceIutStaticPrivateKey, numberOfScenariosPerType);
            }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestsForValidityGroups - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(TestCaseDispositionOption.Success, numberOfSuccessTests);

            // Reorder the test case conditions randomly
            validityTestCaseOptions = validityTestCaseOptions.OrderBy(ob => Guid.NewGuid()).ToList();

            return validityTestCaseOptions;
        }

        public static TestCaseDispositionOption GetTestCaseIntention(List<TestCaseDispositionOption> validityTestCaseOptions)
        {
            var nextDisposition = validityTestCaseOptions[0];
            validityTestCaseOptions.RemoveAt(0);

            return nextDisposition;
        }
    }
}