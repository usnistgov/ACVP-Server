using System.Runtime.Serialization;

namespace NIST.CVP.Common.Oracle.DispositionTypes
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
