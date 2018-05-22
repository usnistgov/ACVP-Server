using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<TestCaseExpectationEnum>
    {
        private List<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<TestCaseExpectationReason>();

            int countForEachCase = (isSample ? 1 : 4);

            _expectationReasons.Add(new TestCaseExpectationReason(TestCaseExpectationEnum.None), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(TestCaseExpectationEnum.OutOfRange), countForEachCase);
            _expectationReasons.Add(new TestCaseExpectationReason(TestCaseExpectationEnum.NotOnCurve), countForEachCase);

            _expectationReasons = _expectationReasons.Shuffle();
        }

        public ITestCaseExpectationReason<TestCaseExpectationEnum> GetRandomReason()
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
