using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SigFailureReasons>
    {
        private List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.None));
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyMessage));
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyKey));
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyR));
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyS));
            }
            else
            {
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.None), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyMessage), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyKey), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyR), 3);
                _expectationReasons.Add(new TestCaseExpectationReason(SigFailureReasons.ModifyS), 3);
            }

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<SigFailureReasons> GetRandomReason()
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
