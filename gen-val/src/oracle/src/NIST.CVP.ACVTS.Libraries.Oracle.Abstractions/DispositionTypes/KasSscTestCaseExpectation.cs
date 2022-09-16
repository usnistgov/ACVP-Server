using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum KasSscTestCaseExpectation
    {
        [EnumMember(Value = "pass")]
        Pass,
        
        [EnumMember(Value = "passLeadingZeroNibble")]
        PassLeadingZeroNibble,
        
        [EnumMember(Value = "failChangedZ")]
        FailChangedZ
    }
}
