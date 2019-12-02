using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class GTestCaseExpectationReason : ITestCaseExpectationReason<DsaGDisposition>
    {
        private readonly DsaGDisposition _reason;

        public GTestCaseExpectationReason(DsaGDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public DsaGDisposition GetReason()
        {
            return _reason;
        }
    }
}
