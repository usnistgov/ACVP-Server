using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SignatureModifications>
    {
        private List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.Message));
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.None));
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.E));
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.Signature));
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.MoveIr));
            _expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.ModifyTrailer));

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<SignatureModifications> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"no {nameof(TestCaseExpectationReason)} remaining to pull");
            }

            var reason = _expectationReasons[0];
            _expectationReasons.RemoveAt(0);

            return reason;
        }
    }
}
