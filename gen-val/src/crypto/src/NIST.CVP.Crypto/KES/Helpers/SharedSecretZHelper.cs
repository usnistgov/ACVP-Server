using System.Numerics;
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
                z = z.PadToModulusMsb(32);
            }
        }

        public static BitString FormatEccSharedSecretZ(BigInteger p, int exactLength)
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

            return new BitString(p).PadToModulusMsb(lengthMod8);
        }
    }
}