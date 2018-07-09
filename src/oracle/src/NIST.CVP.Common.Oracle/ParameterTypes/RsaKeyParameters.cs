using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class RsaKeyParameters
    {
        public PrimeGenModes KeyMode { get; set; }
        public PublicExponentModes PublicExponentMode { get; set; }
        public BitString PublicExponent { get; set; }
        public int Modulus { get; set; }
        public HashFunction HashAlg{ get; set; }
        public PrimeTestModes PrimeTest { get; set; }
        public BitString Seed { get; set; }
        public int[] BitLens { get; set; }

        public PrivateKeyModes KeyFormat { get; set; }
    }
}
