using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations
{
    public class TestCaseExpectationReason : ITestCaseExpectationReason<TestCaseExpectationEnum>
    {
        private readonly TestCaseExpectationEnum _reason;

        public TestCaseExpectationReason(TestCaseExpectationEnum reason)
        {
            _reason = reason;
        }

        public string GetName()
        {
            return EnumHelpers.GetEnumDescriptionFromEnum(_reason);
        }

        public TestCaseExpectationEnum GetReason()
        {
            return _reason;
        }
    }
}
