using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.TestCaseExpectations
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
