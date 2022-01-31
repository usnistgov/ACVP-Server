using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EddsaKeyDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 2 : 5);

            expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.None), countForEachCase);
            expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.NotOnCurve), countForEachCase);

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<EddsaKeyDisposition> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
            }

            _expectationReasons.TryDequeue(out var reason);
            return reason;
        }
    }
}
