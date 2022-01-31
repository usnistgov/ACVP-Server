using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<KasValTestDisposition>
    {
        private readonly KasValTestDisposition _reason;

        public TestCaseExpectationReason(KasValTestDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public KasValTestDisposition GetReason()
        {
            return _reason;
        }
    }
}
