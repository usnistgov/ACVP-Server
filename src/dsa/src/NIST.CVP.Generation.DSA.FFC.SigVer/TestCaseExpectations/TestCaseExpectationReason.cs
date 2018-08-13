using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<DsaSignatureDisposition>
    {
        private readonly DsaSignatureDisposition _reason;

        public TestCaseExpectationReason(DsaSignatureDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public DsaSignatureDisposition GetReason()
        {
            return _reason;
        }
    }
}
