using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<EddsaKeyDisposition>
    {
        private readonly EddsaKeyDisposition _reason;

        public TestCaseExpectationReason(EddsaKeyDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public EddsaKeyDisposition GetReason()
        {
            return _reason;
        }
    }
}
