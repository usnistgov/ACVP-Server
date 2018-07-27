using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations
{
    public class GTestCaseExpectationProvider : ITestCaseExpectationProvider<DsaGDisposition>
    {
        private List<GTestCaseExpectationReason> _expectationReasons;

        public GTestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<GTestCaseExpectationReason>();

            // For a sample case, we always want a 'None' and a 'Modify G'
            if (isSample)
            {
                _expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.None));
                _expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.ModifyG));
            }
            // Otherwise we want everything
            else
            {
                _expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.None), 2);
                _expectationReasons.Add(new GTestCaseExpectationReason(DsaGDisposition.ModifyG), 3);
            }

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<DsaGDisposition> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(GTestCaseExpectationReason)} remaining to pull");
            }

            var reason = _expectationReasons[0];
            _expectationReasons.RemoveAt(0);

            return reason;
        }
    }
}
