using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ECDSA.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<EcdsaKeyDisposition>
    {
        private readonly EcdsaKeyDisposition _reason;

        public TestCaseExpectationReason(EcdsaKeyDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public EcdsaKeyDisposition GetReason()
        {
            return _reason;
        }
    }
}
