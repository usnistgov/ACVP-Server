using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<KasIfcValTestDisposition>
    {
        private readonly KasIfcValTestDisposition _reason;

        public TestCaseExpectationReason(KasIfcValTestDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public KasIfcValTestDisposition GetReason()
        {
            return _reason;
        }
    }
}
