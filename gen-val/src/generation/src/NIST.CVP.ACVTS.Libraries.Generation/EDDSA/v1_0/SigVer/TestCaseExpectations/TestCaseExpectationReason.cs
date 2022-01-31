using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<EddsaSignatureDisposition>
    {
        private readonly EddsaSignatureDisposition _reason;

        public TestCaseExpectationReason(EddsaSignatureDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public EddsaSignatureDisposition GetReason()
        {
            return _reason;
        }
    }
}
