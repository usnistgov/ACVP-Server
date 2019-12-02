using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class PQTestCaseExpectationProvider : ITestCaseExpectationProvider<DsaPQDisposition>
    {
        private readonly List<PQTestCaseExpectationReason> _expectationReasons;

        public PQTestCaseExpectationProvider(bool isSample = false)
        {
            _expectationReasons = new List<PQTestCaseExpectationReason>();

            // For a sample case, we always want a 'None' and a SINGLE random bad reason
            if (isSample)
            {
                var badReasons = new List<PQTestCaseExpectationReason>
                {
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifyP),
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifyQ),
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifySeed)
                };

                _expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.None));
                _expectationReasons.Add(badReasons.OrderBy(a => Guid.NewGuid()).First());
            }
            // Otherwise we want everything
            else
            {
                _expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.None), 2);
                _expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifyP));
                _expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifyQ));
                _expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifySeed));
            }

            _expectationReasons = _expectationReasons.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public ITestCaseExpectationReason<DsaPQDisposition> GetRandomReason()
        {
            if (_expectationReasons.Count == 0)
            {
                throw new IndexOutOfRangeException($"No {nameof(PQTestCaseExpectationReason)} remaining to pull");
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
