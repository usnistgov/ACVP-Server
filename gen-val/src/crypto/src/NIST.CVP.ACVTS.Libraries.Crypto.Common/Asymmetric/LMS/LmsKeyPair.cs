using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class LmsKeyPair
    {
        public LmsPrivateKey PrivateKey { get; set; }
        public BitString PublicKey { get; set; }

        public LmsKeyPair(LmsPrivateKey priv, BitString pub)
        {
            PrivateKey = priv;
            PublicKey = pub;
        }
    }
}
