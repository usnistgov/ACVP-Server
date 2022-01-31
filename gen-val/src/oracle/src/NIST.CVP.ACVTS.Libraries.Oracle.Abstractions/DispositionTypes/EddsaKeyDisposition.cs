using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum EddsaKeyDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "x or y out of range")]
        OutOfRange,

        [EnumMember(Value = "point not on curve")]
        NotOnCurve
    }
}
