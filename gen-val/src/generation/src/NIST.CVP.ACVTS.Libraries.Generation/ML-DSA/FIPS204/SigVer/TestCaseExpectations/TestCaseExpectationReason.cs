using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer.TestCaseExpectations;

public class TestCaseExpectationReason : ITestCaseExpectationReason<MLDSASignatureDisposition>
{
    private readonly MLDSASignatureDisposition _reason;

    public TestCaseExpectationReason(MLDSASignatureDisposition reason)
    {
        _reason = reason;
    }

    public string GetName()
    {
        return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
    }

    public MLDSASignatureDisposition GetReason()
    {
        return _reason;
    }
}
