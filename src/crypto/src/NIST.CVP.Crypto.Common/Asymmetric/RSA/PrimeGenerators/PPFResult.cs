using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public struct PPFResult
    {
        public bool Success;
        public BigInteger P, XP;
        public string ErrorMessage;

        public PPFResult(string fail)
        {
            ErrorMessage = fail;
            Success = false;
            P = XP = 0;
        }

        public PPFResult(BigInteger p, BigInteger xp)
        {
            Success = true;
            P = p;
            XP = xp;
            ErrorMessage = "";
        }
    }

}
