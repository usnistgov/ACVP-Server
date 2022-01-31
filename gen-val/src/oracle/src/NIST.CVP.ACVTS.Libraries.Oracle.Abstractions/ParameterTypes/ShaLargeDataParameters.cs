using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class ShaLargeDataParameters : ShaParameters
    {
        public ExpansionMode ExpansionMode { get; set; }
        public long FullLength { get; set; }
    }
}
