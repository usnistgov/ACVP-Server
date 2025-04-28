using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH
{
    public class XecdhKeyPair
    {
        public BitString PublicKey { get; set; }
        public BitString PrivateKey { get; set; }

        public XecdhKeyPair()
        {

        }

        public XecdhKeyPair(BitString publicKey, BitString privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
