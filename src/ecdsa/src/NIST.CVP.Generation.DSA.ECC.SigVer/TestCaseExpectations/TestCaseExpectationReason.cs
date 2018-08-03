using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<EcdsaSignatureDisposition>
    {
        private readonly EcdsaSignatureDisposition _reason;

        public TestCaseExpectationReason(EcdsaSignatureDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public EcdsaSignatureDisposition GetReason()
        {
            return _reason;
        }
    }
}
