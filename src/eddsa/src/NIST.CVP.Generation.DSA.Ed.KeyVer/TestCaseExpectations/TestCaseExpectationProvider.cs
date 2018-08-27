using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.Ed.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EddsaKeyDisposition>
    {
        private readonly List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 1 : 4);

            _expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.None), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.OutOfRange), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(EddsaKeyDisposition.NotOnCurve), countForEachCase);

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<EddsaKeyDisposition> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
            }

            var reason = _expectationReasons[0];
            _expectationReasons.RemoveAt(0);

            return reason;
        }
    }
}
