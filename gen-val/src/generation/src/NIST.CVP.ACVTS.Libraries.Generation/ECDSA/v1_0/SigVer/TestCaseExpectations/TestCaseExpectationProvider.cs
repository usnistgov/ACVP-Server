﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationProvider : ITestCaseExpectationProvider<EcdsaSignatureDisposition>
    {
        private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

        public TestCaseExpectationProvider(bool isSample = false)
        {
            var expectationReasons = new List<TestCaseExpectationReason>();

            if (isSample)
            {
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.None));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyMessage));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyKey));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyR));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyS));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ZeroR));
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ZeroS));
            }
            else
            {
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.None), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyMessage), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyKey), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyR), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ModifyS), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ZeroR), 3);
                expectationReasons.Add(new TestCaseExpectationReason(EcdsaSignatureDisposition.ZeroS), 3);
            }

            _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
        }

        public int ExpectationCount => _expectationReasons.Count;

        public ITestCaseExpectationReason<EcdsaSignatureDisposition> GetRandomReason()
        {
            if (_expectationReasons.TryDequeue(out var reason))
            {
                return reason;
            }

            throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
        }
    }
}
