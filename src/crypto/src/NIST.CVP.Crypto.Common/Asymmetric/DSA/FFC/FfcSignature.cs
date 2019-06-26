using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcSignature : IDsaSignature
    {
        public BitString R { get; set; }
        public BitString S { get; set; }

        public FfcSignature()
        {
            
        }

        public FfcSignature(BitString r, BitString s)
        {
            R = r;
            S = s;
        }
    }
}
