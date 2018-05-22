using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public struct PPCResult
    {
        public bool Success;
        public BigInteger P, P1, P2, PSeed;
        public string ErrorMessage;

        public PPCResult(string fail)
        {
            ErrorMessage = fail;
            Success = false;
            P = P1 = P2 = PSeed = 0;
        }

        public PPCResult(BigInteger p, BigInteger p1, BigInteger p2, BigInteger pSeed)
        {
            Success = true;
            P = p;
            P1 = p1;
            P2 = p2;
            PSeed = pSeed;
            ErrorMessage = "";
        }
    }

}
