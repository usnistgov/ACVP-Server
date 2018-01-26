using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.Helpers
{
    public static class DSAHelper
    {
        public static bool VerifyLenPair(int L, int N)
        {
            if (L == 1024)
            {
                return (N == 160);
            }
            else if (L == 2048)
            {
                return (N == 224 || N == 256);
            }
            else if (L == 3072)
            {
                return (N == 256);
            }

            return false;
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

            return 1;
        }
    }
}
