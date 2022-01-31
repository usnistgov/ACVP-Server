using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.TestCaseExpectations
{
    public class TestCaseExpectationProvider<TTestGroup, TTestCase, TKeyPair> : ITestCaseExpectationProvider<KasValTestDisposition>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>
        where TKeyPair : IDsaKeyPair
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount => _expectationReasons.Count;

        public TestCaseExpectationProvider(TestGroupBase<TTestGroup, TTestCase, TKeyPair> testGroup, bool includeFailureTests)
        {
            const int numberOfScenariosPerType = 2;
            var validityTestCaseOptions = new List<TestCaseExpectationReason>();

            var numberOfTestCases = includeFailureTests ? 25 : 10;

            if (!includeFailureTests)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.Success), numberOfTestCases);
                _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(validityTestCaseOptions.Shuffle());
                return;
            }

            // Can introduce errors/conditions into Oi, Dkm, MacData
            if (testGroup.KasMode != KasMode.NoKdfNoKc)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedDkm), numberOfScenariosPerType);
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedMacData), numberOfScenariosPerType);
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.SuccessLeadingZeroNibbleDkm), 1);
            }

            if (testGroup.KasMode == KasMode.KdfKc)
            {
                validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedTag), numberOfScenariosPerType);
            }

            // Can always introduce errors/conditions into Z / tag (or hash tag)
            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.FailChangedZ), numberOfScenariosPerType);
            validityTestCaseOptions.Add(new TestCaseExpectationReason(KasValTestDisposition.SuccessLeadingZeroNibbleZ), 1);

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
