using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Enums
{
    public enum TestCaseExpectationEnum
    {
        [Description("none")]
        None,

        [Description("x or y out of range")]
        OutOfRange,

        [Description("point not on curve")]
        NotOnCurve
    }
}
