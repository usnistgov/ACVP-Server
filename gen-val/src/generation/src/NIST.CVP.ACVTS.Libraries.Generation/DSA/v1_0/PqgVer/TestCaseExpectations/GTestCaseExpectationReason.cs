using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
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
