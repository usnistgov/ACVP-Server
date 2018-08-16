using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdSignature : IDsaSignature
    {
        public BigInteger Sig { get; set; }

        public EdSignature()
        {
            
        }

        public EdSignature(BigInteger sig)
        {
            Sig = sig;
        }
    }
}
