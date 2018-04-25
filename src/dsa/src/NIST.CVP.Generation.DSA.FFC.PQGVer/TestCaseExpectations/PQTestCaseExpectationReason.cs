using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations
{
    public class PQTestCaseExpectationReason : ITestCaseExpectationReason<PQFailureReasons>
    {
        private readonly PQFailureReasons _reason;

        public PQTestCaseExpectationReason(PQFailureReasons reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public PQFailureReasons GetReason()
        {
            return _reason;
        }
    }
}
