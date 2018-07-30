using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EcdsaKeyDisposition>
    {
        private readonly List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 1 : 4);

            _expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.None), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.OutOfRange), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(EcdsaKeyDisposition.NotOnCurve), countForEachCase);

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<EcdsaKeyDisposition> GetRandomReason()
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
