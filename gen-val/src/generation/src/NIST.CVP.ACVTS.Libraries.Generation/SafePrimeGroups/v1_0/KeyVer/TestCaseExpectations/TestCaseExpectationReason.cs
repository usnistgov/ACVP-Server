using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<SafePrimesKeyDisposition>
    {
        private readonly SafePrimesKeyDisposition _reason;

        public TestCaseExpectationReason(SafePrimesKeyDisposition reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public SafePrimesKeyDisposition GetReason()
        {
            return _reason;
        }
    }
}
