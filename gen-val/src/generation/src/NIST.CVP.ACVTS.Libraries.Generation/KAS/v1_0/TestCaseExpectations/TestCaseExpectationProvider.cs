using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.TestCaseExpectations
{
    public class TestCaseExpectationProvider<TTestGroup, TTestCase, TKasAlgoAttributes> : ITestCaseExpectationProvider<KasValTestDisposition>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKasAlgoAttributes>
        where TKasAlgoAttributes : IKasAlgoAttributes
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount => _expectationReasons.Count;

        public TestCaseExpectationProvider(TestGroupBase<TTestGroup, TTestCase, TKasAlgoAttributes> testGroup)
        {
            var validityTestCaseOptions = new List<TestCaseExpectationReason>();
            const int numberOfTestsForValidityGroups = 25;
            const int numberOfScenariosPerType = 2;

            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedOi), numberOfScenariosPerType);
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedDkm), numberOfScenariosPerType);
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedMacData), numberOfScenariosPerType);
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.SuccessLeadingZeroNibbleDkm), numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash tag)
            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedZ), numberOfScenariosPerType);
            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.SuccessLeadingZeroNibbleZ), numberOfScenariosPerType);
            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedTag), numberOfScenariosPerType);

            // Conditions based on assurances
            if ((testGroup.Function.HasFlag(KasAssurance.FullVal))
                && testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailAssuranceIutStaticPublicKey), numberOfScenariosPerType);
            }

            if (testGroup.Function.HasFlag(KasAssurance.FullVal))
            {
                if (testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
                {
                    validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailAssuranceServerStaticPublicKey),
                        numberOfScenariosPerType);
                }

                if (testGroup.KeyNonceGenRequirementsServer.GeneratesEphemeralKeyPair)
                {
                    validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailAssuranceServerEphemeralPublicKey),
                        numberOfScenariosPerType);
                }
            }

            if (testGroup.Function.HasFlag(KasAssurance.PartialVal)
                && testGroup.KeyNonceGenRequirementsServer.GeneratesStaticKeyPair)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailAssuranceServerStaticPublicKey), numberOfScenariosPerType);
            }

            // Determine number of successful cases to generate
            int numberOfSuccessTests = numberOfTestsForValidityGroups - validityTestCaseOptions.Count;

            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.Success), numberOfSuccessTests);

            // Reorder the test case conditions randomly
            validityTestCaseOptions = validityTestCaseOptions.OrderBy(ob => Guid.NewGuid()).ToList();

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(validityTestCaseOptions.Shuffle());
        }

        public ITestCaseExpectationReason<KasValTestDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
