using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent.TestCaseExpectations{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<RsaDpDisposition>
    {
        private readonly RsaDpDisposition _reason;
        
        public TestCaseExpectationReason(RsaDpDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public RsaDpDisposition GetReason()
        {
            return _reason;
        }
    }
}
