using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<LmsSignatureDisposition>
    {
        private readonly LmsSignatureDisposition _reason;

        public TestCaseExpectationReason(LmsSignatureDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public LmsSignatureDisposition GetReason()
        {
            return _reason;
        }
    }
}
