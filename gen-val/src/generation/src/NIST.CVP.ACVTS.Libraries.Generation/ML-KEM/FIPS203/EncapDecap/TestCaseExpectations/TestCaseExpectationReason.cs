using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap.TestCaseExpectations;

public class TestCaseExpectationReason : ITestCaseExpectationReason<MLKEMDecapsulationDisposition>
{
    private readonly MLKEMDecapsulationDisposition _reason;

    public TestCaseExpectationReason(MLKEMDecapsulationDisposition reason)
    {
        _reason = reason;
    }

    public string GetName()
    {
        return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
    }

    public MLKEMDecapsulationDisposition GetReason()
    {
        return _reason;
    }
}
