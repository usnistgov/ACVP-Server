using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.TestCaseExpectations
{
    public class TestCaseExpecttionReason : ITestCaseExpectationReason<KasSscTestCaseExpectation>
    {
        private readonly KasSscTestCaseExpectation _reason;

        public TestCaseExpecttionReason(KasSscTestCaseExpectation reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public KasSscTestCaseExpectation GetReason()
        {
            return _reason;
        }
    }
}
