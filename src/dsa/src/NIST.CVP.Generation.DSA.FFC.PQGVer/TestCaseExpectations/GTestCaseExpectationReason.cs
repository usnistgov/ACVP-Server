using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations
{
    public class GTestCaseExpectationReason : ITestCaseExpectationReason<GFailureReasons>
    {
        private readonly GFailureReasons _reason;

        public GTestCaseExpectationReason(GFailureReasons reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public GFailureReasons GetReason()
        {
            return _reason;
        }
    }
}
