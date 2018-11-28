using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaKeyResult : IResult
    {
        public KeyPair Key { get; set; }

        public BitString Seed { get; set; }
        public int[] BitLens { get; set; }
        public AuxiliaryResult AuxValues { get; set; }
    }
}
