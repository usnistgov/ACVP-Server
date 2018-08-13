using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EcdsaSignatureDisposition>
    {
        private List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.None));
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyMessage));
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyKey));
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyR));
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyS));
            }
            else
            {
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.None), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyMessage), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyKey), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyR), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyS), 3);
            }

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<EcdsaSignatureDisposition> GetRandomReason()
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
