using NIST.CVP.Crypto.Common.KDF.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KdfParameters
    {
        public int KeyOutLength { get; set; }
        public bool ZeroLengthIv { get; set; }
        public KdfModes Mode { get; set; }
        public MacModes MacMode { get; set; }
        public int CounterLength { get; set; }
        public CounterLocations CounterLocation { get; set; }
    }
}
