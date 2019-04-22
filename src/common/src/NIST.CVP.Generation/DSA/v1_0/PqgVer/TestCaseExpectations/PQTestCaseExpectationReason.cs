using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer.TestCaseExpectations
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
