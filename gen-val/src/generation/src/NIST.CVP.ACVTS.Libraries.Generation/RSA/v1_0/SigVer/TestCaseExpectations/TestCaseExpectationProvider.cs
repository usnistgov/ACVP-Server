﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SignatureModifications>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.Message));
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.None));
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.E));
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.Signature));
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.MoveIr));
            expectationReasons.Add(new TestCaseExpectationReason(SignatureModifications.ModifyTrailer));

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<SignatureModifications> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
        }
    }
}
