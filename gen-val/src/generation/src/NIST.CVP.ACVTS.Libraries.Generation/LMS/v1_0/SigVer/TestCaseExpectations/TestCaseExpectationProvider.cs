using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer.TestCaseExpectations;

    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<LmsSignatureDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>
            {
                    new(LmsSignatureDisposition.None),
                    new(LmsSignatureDisposition.ModifySignature),
                    new(LmsSignatureDisposition.ModifyKey),
                    new(LmsSignatureDisposition.ModifyMessage),
                    new(LmsSignatureDisposition.ModifyHeader)
                };

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<LmsSignatureDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
        }
    }
