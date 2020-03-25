using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EddsaKeyDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 1 : 4);

            expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.None), countForEachCase);
            expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.OutOfRange), countForEachCase);
            expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.NotOnCurve), countForEachCase);

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

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
