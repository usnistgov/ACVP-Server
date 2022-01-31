using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class PQTestCaseExpectationProvider : ITestCaseExpectationProvider<DsaPQDisposition>
    {
        private readonly ConcurrentQueue<PQTestCaseExpectationReason> _expectationReasons;

        public PQTestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<PQTestCaseExpectationReason>();

            // For a sample case, we always want a 'None' and a SINGLE random bad reason
            if (isSample)
            {
                var badReasons = new List<PQTestCaseExpectationReason>
                {
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifyP),
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifyQ),
                    new PQTestCaseExpectationReason(DsaPQDisposition.ModifySeed)
                };

                expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.None));
                expectationReasons.Add(badReasons.OrderBy(a => Guid.NewGuid()).First());
            }
            // Otherwise we want everything
            else
            {
                expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.None), 2);
                expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifyP));
                expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifyQ));
                expectationReasons.Add(new PQTestCaseExpectationReason(DsaPQDisposition.ModifySeed));
            }

            _expectationReasons = new ConcurrentQueue<PQTestCaseExpectationReason>(expectationReasons.OrderBy(a => Guid.NewGuid()).ToList());
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<DsaPQDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
