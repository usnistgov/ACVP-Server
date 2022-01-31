using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class KdfParameters
    {
        public int IvLength { get; set; }
        public int KeyInLength { get; set; }
        public int KeyOutLength { get; set; }
        public bool ZeroLengthIv { get; set; }
        public KdfModes Mode { get; set; }
        public MacModes MacMode { get; set; }
        public int CounterLength { get; set; }
        public CounterLocations CounterLocation { get; set; }
    }
}
