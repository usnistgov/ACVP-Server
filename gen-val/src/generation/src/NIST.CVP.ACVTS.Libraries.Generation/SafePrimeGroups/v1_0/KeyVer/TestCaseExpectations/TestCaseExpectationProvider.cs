using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SafePrimesKeyDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public int ExpectationCount => _expectationReasons.Count;

        public TestCaseExpectationProvider(bool isSample)
        {
            var list = new List<TestCaseExpectationReason>();
            var numberOfTestCases = isSample ? 25 : 10;

            var numberOfFailures = numberOfTestCases.CeilingDivide(4);
            var numberOfSuccesses = numberOfTestCases - numberOfFailures;

            list.Add(new TestCaseExpectationReason(SafePrimesKeyDisposition.Valid), numberOfSuccesses);
            list.Add(new TestCaseExpectationReason(SafePrimesKeyDisposition.Invalid), numberOfFailures);

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(list.Shuffle());
        }

        public ITestCaseExpectationReason<SafePrimesKeyDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
