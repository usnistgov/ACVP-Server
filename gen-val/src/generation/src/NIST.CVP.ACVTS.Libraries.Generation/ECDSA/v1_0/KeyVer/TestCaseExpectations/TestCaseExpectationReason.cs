using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyVer.TestCaseExpectations
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
