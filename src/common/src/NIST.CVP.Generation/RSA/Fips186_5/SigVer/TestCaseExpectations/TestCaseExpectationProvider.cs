using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SignatureModifications>
    {
        private readonly List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>
            {
                new TestCaseExpectationReason(SignatureModifications.Message),
                new TestCaseExpectationReason(SignatureModifications.None),
                new TestCaseExpectationReason(SignatureModifications.E),
                new TestCaseExpectationReason(SignatureModifications.Signature),
                new TestCaseExpectationReason(SignatureModifications.MoveIr),
                new TestCaseExpectationReason(SignatureModifications.ModifyTrailer)
            };

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<SignatureModifications> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"no {nameof(TestCaseExpectationReason)} remaining to pull");
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