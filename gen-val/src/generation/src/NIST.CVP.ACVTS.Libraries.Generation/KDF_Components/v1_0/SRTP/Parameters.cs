using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public int[] AesKeyLength { get; set; }
        public bool SupportsZeroKdr { get; set; }
        public int[] KdrExponent { get; set; }
        public bool? Supports48BitSrtcpIndex { get; set; }
    }
}
