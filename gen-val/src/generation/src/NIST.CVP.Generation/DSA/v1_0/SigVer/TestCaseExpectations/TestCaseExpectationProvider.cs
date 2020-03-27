using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<DsaSignatureDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.None));
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyMessage));
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyKey));
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyR));
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyS));
            }
            else
            {
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.None), 7);
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyMessage), 2);
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyKey), 2);
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyR), 2);
                expectationReasons.Add(new TestCaseExpectationReason(DsaSignatureDisposition.ModifyS), 2);
            }

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.OrderBy(a => Guid.NewGuid()).ToList());
        }

        public ITestCaseExpectationReason<DsaSignatureDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }
            
            throw new IndexOutOfRangeException($"No {nameof(_expectationReasons)} remaining to pull");
        }
    }
}
