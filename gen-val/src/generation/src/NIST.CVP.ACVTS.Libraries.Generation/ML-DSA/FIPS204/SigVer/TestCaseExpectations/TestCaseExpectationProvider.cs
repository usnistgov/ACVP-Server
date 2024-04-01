using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer.TestCaseExpectations;

public class TestCaseExpectationProvider : ITestCaseExpectationProvider<MLDSASignatureDisposition>
{
    private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

    public TestCaseExpectationProvider(bool isSample = false)
    {
        var expectationReasons = new List<TestCaseExpectationReason>();

        expectationReasons.Add(new TestCaseExpectationReason(MLDSASignatureDisposition.None), 3);
        expectationReasons.Add(new TestCaseExpectationReason(MLDSASignatureDisposition.ModifyMessage), 3);
        expectationReasons.Add(new TestCaseExpectationReason(MLDSASignatureDisposition.ModifySignature), 3);
        expectationReasons.Add(new TestCaseExpectationReason(MLDSASignatureDisposition.ModifyHint), 3);
        expectationReasons.Add(new TestCaseExpectationReason(MLDSASignatureDisposition.ModifyZ), 3);

        _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
    }

    public int ExpectationCount => _expectationReasons.Count;

    public ITestCaseExpectationReason<MLDSASignatureDisposition> GetRandomReason()
    {
        if (_expectationReasons.TryDequeue(out var reason))
        {
            return reason;
        }

        throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
    }
}
