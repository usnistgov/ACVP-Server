using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<KdaTestCaseDisposition>
    {
        private readonly KdaTestCaseDisposition _reason;

        public TestCaseExpectationReason(KdaTestCaseDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public KdaTestCaseDisposition GetReason()
        {
            return _reason;
        }
    }
}
