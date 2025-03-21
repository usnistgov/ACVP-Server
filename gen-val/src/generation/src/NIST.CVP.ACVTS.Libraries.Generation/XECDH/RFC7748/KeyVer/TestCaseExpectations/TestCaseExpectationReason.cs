using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<XecdhKeyDisposition>
    {
        private readonly XecdhKeyDisposition _reason;

        public TestCaseExpectationReason(XecdhKeyDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public XecdhKeyDisposition GetReason()
        {
            return _reason;
        }
    }
}
