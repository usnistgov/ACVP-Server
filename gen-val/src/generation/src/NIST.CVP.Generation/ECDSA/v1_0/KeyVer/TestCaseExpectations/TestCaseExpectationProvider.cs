using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NIST.CVP.Generation.ECDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EcdsaKeyDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 1 : 4);

            expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.None), countForEachCase);
            expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.OutOfRange), countForEachCase);
            expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.NotOnCurve), countForEachCase);

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public ITestCaseExpectationReason<EcdsaKeyDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }
            
            throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
        }
    }
}
