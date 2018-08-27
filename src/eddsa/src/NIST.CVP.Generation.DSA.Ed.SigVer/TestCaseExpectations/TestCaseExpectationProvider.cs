using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EddsaSignatureDisposition>
    {
        private List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.None));
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyMessage));
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyKey));
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyR));
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyS));
            }
            else
            {
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.None), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyMessage), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyKey), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyR), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(EddsaSignatureDisposition.ModifyS), 3);
            }

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<EddsaSignatureDisposition> GetRandomReason()
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
