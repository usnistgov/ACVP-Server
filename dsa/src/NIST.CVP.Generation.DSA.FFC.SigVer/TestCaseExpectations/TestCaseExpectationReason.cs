using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<SigFailureReasons>
    {
        private readonly SigFailureReasons _reason;

        public TestCaseExpectationReason(SigFailureReasons reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public SigFailureReasons GetReason()
        {
            return _reason;
        }
    }
}
