using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class HssKeyPair
    {
        public HssPrivateKey PrivateKey { get; set; }
        public BitString PublicKey { get; set; }
        public bool Expired { get; set; }

        public HssKeyPair(HssPrivateKey priv, BitString publicKey)
        {
            PrivateKey = priv;
            PublicKey = publicKey;
            Expired = false;
        }

        public HssKeyPair GetDeepCopy()
        {
            var copy = new HssKeyPair(PrivateKey.GetDeepCopy(), PublicKey.GetDeepCopy());
            copy.Expired = Expired;
            return copy;
        }
    }
}
