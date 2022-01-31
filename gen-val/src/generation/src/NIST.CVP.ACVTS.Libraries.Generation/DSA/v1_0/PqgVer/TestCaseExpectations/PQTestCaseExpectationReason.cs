using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
{
    public class PQTestCaseExpectationReason : ITestCaseExpectationReason<DsaPQDisposition>
    {
        private readonly DsaPQDisposition _reason;

        public PQTestCaseExpectationReason(DsaPQDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public DsaPQDisposition GetReason()
        {
            return _reason;
        }
    }
}
