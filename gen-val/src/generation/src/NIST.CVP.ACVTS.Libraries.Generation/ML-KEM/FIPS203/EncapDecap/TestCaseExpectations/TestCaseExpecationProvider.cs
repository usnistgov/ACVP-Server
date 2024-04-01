using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

public class TestCaseExpectationProvider : ITestCaseExpectationProvider<MLKEMDecapsulationDisposition>
{
    private readonly ConcurrentQueue<TestCaseExpectationReason> _expectationReasons;

    public TestCaseExpectationProvider(bool isSample = false)
    {
        var expectationReasons = new List<TestCaseExpectationReason>();

        expectationReasons.Add(new TestCaseExpectationReason(MLKEMDecapsulationDisposition.None), 5);
        expectationReasons.Add(new TestCaseExpectationReason(MLKEMDecapsulationDisposition.ModifyCiphertext), 5);

        _expectationReasons = new ConcurrentQueue<TestCaseExpectationReason>(expectationReasons.Shuffle());
    }

    public int ExpectationCount => _expectationReasons.Count;

    public ITestCaseExpectationReason<MLKEMDecapsulationDisposition> GetRandomReason()
    {
        if (_expectationReasons.TryDequeue(out var reason))
        {
            return reason;
        }

        throw new IndexOutOfRangeException($"No {nameof(TestCaseExpectationReason)} remaining to pull");
    }
}
