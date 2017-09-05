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
    }
}
