using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.TestCaseExpectations;

public class TestCaseExpectationProvider : ITestCaseExpectationProvider<SLHDSASignatureDisposition>
{
    private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

    public TestCaseExpectationProvider(bool isSample = false)
    {
        var expectationReasons = new List<TestCaseExpectationReason>();

        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.None), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifyMessage), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifySignatureR), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifySignatureSigFors), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifySignatureSigHt), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifySignatureTooLarge), 2);
        expectationReasons.Add(new TestCaseExpectationReason(SLHDSASignatureDisposition.ModifySignatureTooSmall), 2);

        _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
    }

    public int ExpectationCount => _expectationReasons.Count;

    public ITestCaseExpectationReason<SLHDSASignatureDisposition> GetRandomReason()
    {
        if (_expectationReasons.TryDequeue(out var reason))
        {
            return reason;
        }

        throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
    }
}
