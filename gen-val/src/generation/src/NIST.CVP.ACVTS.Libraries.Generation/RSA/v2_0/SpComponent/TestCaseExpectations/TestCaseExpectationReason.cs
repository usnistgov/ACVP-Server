using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<RsaSpDisposition>
    {
        private readonly RsaSpDisposition _reason;
        
        public TestCaseExpectationReason(RsaSpDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public RsaSpDisposition GetReason()
        {
            return _reason;
        }
    }
}