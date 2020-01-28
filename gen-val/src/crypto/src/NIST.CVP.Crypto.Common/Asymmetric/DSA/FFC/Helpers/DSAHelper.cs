using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers
{
    public static class DSAHelper
    {
        public static bool VerifyLenPair(int L, int N)
        {
            switch (L)
            {
                case 1024:
                    return (N == 160);
                case 2048:
                    return (N == 224 || N == 256 || N == 2048);
                case 3072:
                    return (N == 256);
                default:
                    return false;
            }
        }

        public static int GetMillerRabinIterations(int L, int N)
        {
            if (L == 1024)
            {
                return 40;
            }
            else if(L == 2048)
            {
                return 56;
            }
            else if(L == 3072)
            {
                return 64;
            }

            throw new Exception("Not a valid L");
        }
    }
}
