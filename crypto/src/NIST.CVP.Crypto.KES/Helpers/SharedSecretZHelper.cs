using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES.Helpers
{
    public static class SharedSecretZHelper
    {
        public static void FormatFfcSharedSecretZ(ref BitString z)
        {
            if (z.BitLength % 32 != 0)
            {
                z = BitString.ConcatenateBits(BitString.Zeroes(32 - z.BitLength % 32), z);
            }
        }

        public static BitString FormatEccSharedSecretZ(EccPoint p, int exactLength)
        {
            int lengthMod8;
            if (exactLength % 8 != 0)
            {
                lengthMod8 = exactLength + 8 - (exactLength % 8);
            }
            else
            {
                lengthMod8 = exactLength;
            }

            var z = new BitString(p.X, lengthMod8);
            return z;
        }
    }
}