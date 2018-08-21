using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.TestCaseExpectations
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
