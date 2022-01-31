using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<KasKcDisposition>
    {
        private readonly KasKcDisposition _reason;

        public TestCaseExpectationReason(KasKcDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public KasKcDisposition GetReason()
        {
            return _reason;
        }
    }
}
