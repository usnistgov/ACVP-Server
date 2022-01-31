using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<KdaTestCaseDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount => _expectationReasons.Count;

        public TestCaseExpectationProvider(bool isSample)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                var totalTests = 5;

                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.Fail));
                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.SuccessLeadingZeroNibble));

                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.Success), totalTests - expectationReasons.Count);
            }
            else
            {
                var totalTests = 15;

                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.Fail), 2);
                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.SuccessLeadingZeroNibble), 2);

                expectationReasons.Add(new TestCaseExpectationReason(KdaTestCaseDisposition.Success), totalTests - expectationReasons.Count);
            }

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public ITestCaseExpectationReason<KdaTestCaseDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
