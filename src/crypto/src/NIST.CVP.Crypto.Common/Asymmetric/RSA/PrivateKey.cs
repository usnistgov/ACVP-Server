using NIST.CVP.Math.Helpers;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA
{
    public class PrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger D { get; set; }
        public BigInteger Phi_N => (P - 1) * (Q - 1);

        public BigInteger DMP1 { get; set; }
        public BigInteger DMQ1 { get; set; }
        public BigInteger IQMP { get; set; }

        public void ComputeCRT()
        {
            DMP1 = D % (P - 1);
            DMQ1 = D % (Q - 1);
            IQMP = Q.ModularInverse(P);
        }
    }
}
