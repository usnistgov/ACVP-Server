using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SRTP
{
    public class SrtpResult
    {
        public BitString AuthenticationKey { get; set; }
        public BitString EncryptionKey { get; set; }
        public BitString SaltingKey { get; set; }

        public SrtpResult(BitString ek, BitString ak, BitString sk)
        {
            AuthenticationKey = ak;
            EncryptionKey = ek;
            SaltingKey = sk;
        }
    }
}
