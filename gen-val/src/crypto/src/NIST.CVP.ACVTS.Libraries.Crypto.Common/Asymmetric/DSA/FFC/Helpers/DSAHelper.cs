using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Helpers
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
                    return (N == 224 || N == 256 || N == L);
                case 3072:
                    return (N == 256 || N == L);
                case 4096:
                case 6144:
                case 8192:
                    return (N == L);
                default:
                    return false;
            }
        }

        public static int GetMillerRabinIterations(int L, int N)
        {
            switch (L)
            {
                case 1024:
                    return 40;
                case 2048:
                    return 56;
                case 3072:
                case 4096:
                case 6144:
                case 8192:
                    return 64;
                default:
                    throw new Exception("Not a valid L");
            }
        }
    }
}
