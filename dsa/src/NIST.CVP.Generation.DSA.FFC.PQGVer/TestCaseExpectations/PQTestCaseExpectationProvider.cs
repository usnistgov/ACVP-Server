using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations
{
    public class PQTestCaseExpectationProvider : ITestCaseExpectationProvider<PQFailureReasons>
    {
        private List<PQTestCaseExpectationReason> _expectationReasons;

        public PQTestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<PQTestCaseExpectationReason>();

            // For a sample case, we always want a 'None' and a SINGLE random bad reason
            if (isSample)
            {
                var badReasons = new List<PQTestCaseExpectationReason>
                {
                    new PQTestCaseExpectationReason(PQFailureReasons.ModifyP),
                    new PQTestCaseExpectationReason(PQFailureReasons.ModifyQ),
                    new PQTestCaseExpectationReason(PQFailureReasons.ModifySeed)
                };

                _expectationReasons.Add(new PQTestCaseExpectationReason(PQFailureReasons.None));
                _expectationReasons.Add(badReasons.OrderBy(a => Guid.NewGuid()).ToList()[0]);
            }
            // Otherwise we want everything
            else
            {
                _expectationReasons.Add(new PQTestCaseExpectationReason(PQFailureReasons.None), 2);
                _expectationReasons.Add(new PQTestCaseExpectationReason(PQFailureReasons.ModifyP));
                _expectationReasons.Add(new PQTestCaseExpectationReason(PQFailureReasons.ModifyQ));
                _expectationReasons.Add(new PQTestCaseExpectationReason(PQFailureReasons.ModifySeed));
            }

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<PQFailureReasons> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(PQTestCaseExpectationReason)} remaining to pull");
            }

            var reason = _expectationReasons[0];
            _expectationReasons.RemoveAt(0);

            return reason;
        }
    }
}
