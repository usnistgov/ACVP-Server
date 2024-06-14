using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer.TestCaseExpectations;

public class TestCaseExpectationReason : ITestCaseExpectationReason<SLHDSASignatureDisposition>
{
    private readonly SLHDSASignatureDisposition _reason;

    public TestCaseExpectationReason(SLHDSASignatureDisposition reason)
    {
        _reason = reason;
    }

    public string GetName()
    {
        return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
    }

    public SLHDSASignatureDisposition GetReason()
    {
        return _reason;
    }
}
