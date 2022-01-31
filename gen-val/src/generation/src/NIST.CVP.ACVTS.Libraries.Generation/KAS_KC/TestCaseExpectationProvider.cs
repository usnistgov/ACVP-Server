using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<KasKcDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount => _expectationReasons.Count;

        public TestCaseExpectationProvider(bool isSample)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            var totalCases = 25;
            var totalPerNonSuccessScenario = 4;
            if (isSample)
            {
                totalCases = 10;
                totalPerNonSuccessScenario = 1;
            }

            var totalSuccessScenarios = totalCases - totalPerNonSuccessScenario;

            expectationReasons.Add(new TestCaseExpectationReason(KasKcDisposition.LeadingOneBitKey), totalPerNonSuccessScenario);
            expectationReasons.Add(new TestCaseExpectationReason(KasKcDisposition.LeadingZeroByteKey), totalPerNonSuccessScenario);
            expectationReasons.Add(new TestCaseExpectationReason(KasKcDisposition.LeadingZeroNibbleKey), totalPerNonSuccessScenario);

            expectationReasons.Add(new TestCaseExpectationReason(KasKcDisposition.Success), totalSuccessScenarios);

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public ITestCaseExpectationReason<KasKcDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
