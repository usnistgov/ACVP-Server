using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Enums
{
    public enum TestCaseExpectationEnum
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "x or y out of range")]
        OutOfRange,

        [EnumMember(Value = "point not on curve")]
        NotOnCurve
    }
}
