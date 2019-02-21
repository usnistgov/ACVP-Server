using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdSignature : IDsaSignature
    {
        private BitString _sig;
        public BitString Sig
        {
            get => _sig ?? R.ConcatenateBits(S);
            set => _sig = value.GetDeepCopy();
        }

        public BitString R { get; set; }
        public BitString S { get; set; }

        public EdSignature()
        {
            
        }

        public EdSignature(BitString sig)
        {
            Sig = sig.GetDeepCopy();
        }

        public EdSignature(BitString r, BitString s)
        {
            R = r.GetDeepCopy();
            S = s.GetDeepCopy();
        }
    }
}
