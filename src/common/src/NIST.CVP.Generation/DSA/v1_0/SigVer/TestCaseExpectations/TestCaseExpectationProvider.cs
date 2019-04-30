using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<DsaSignatureDisposition>
    {
        private readonly List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.None));
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyMessage));
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyKey));
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyR));
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyS));
            }
            else
            {
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.None), 7);
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyMessage), 2);
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyKey), 2);
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyR), 2);
                _expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyS), 2);
            }

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<DsaSignatureDisposition> GetRandomReason()
        {
            if (_expectationReasons == null)
            {
                throw new Exception("ExpectationReasons is null");
            }
            
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
