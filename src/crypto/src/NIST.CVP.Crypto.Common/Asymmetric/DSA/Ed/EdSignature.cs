using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdSignature : IDsaSignature
    {
        public BitString Sig { get; set; }

        public EdSignature()
        {
            Sig = new BitString("");
        }

        public EdSignature(BitString sig)
        {
            Sig = sig.GetDeepCopy();
        }

        public EdSignature(BitString r, BitString s)
        {
            Sig = r.ConcatenateBits(s);
        }
    }
}
