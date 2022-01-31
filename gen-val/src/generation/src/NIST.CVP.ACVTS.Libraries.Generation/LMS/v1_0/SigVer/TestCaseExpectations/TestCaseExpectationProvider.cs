using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<LmsSignatureDisposition>
    {
        private readonly List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.None));
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifyMessage));
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifyKey));
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifySignature));
            }
            else
            {
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.None), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifyMessage), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifyKey), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(LmsSignatureDisposition.ModifySignature), 3);
            }

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<LmsSignatureDisposition> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
            }

            lock (_expectationReasons)
            {
                var reason = _expectationReasons[0];
                _expectationReasons.RemoveAt(0);

                return reason;
            }
        }
    }
}
