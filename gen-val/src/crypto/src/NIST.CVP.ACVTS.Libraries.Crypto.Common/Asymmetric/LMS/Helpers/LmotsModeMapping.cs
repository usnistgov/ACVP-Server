using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Helpers
{
    public class LmotsModeMapping
    {
        public static (int n, int w, int p, int ls, int siglen, BitString typecode) GetValsFromType(LmotsType type)
        {
            if (type == LmotsType.LMOTS_SHA256_N32_W1)
            {
                return (32, 1, 265, 7, 8516, new BitString(1, 32));
            }
            else if (type == LmotsType.LMOTS_SHA256_N32_W2)
            {
                return (32, 2, 133, 6, 4292, new BitString(2, 32));
            }
            else if (type == LmotsType.LMOTS_SHA256_N32_W4)
            {
                return (32, 4, 67, 4, 2180, new BitString(3, 32));
            }
            else
            {
                return (32, 8, 34, 0, 1124, new BitString(4, 32));
            }
        }

        public static int GetNFromCode(BitString typecode)
        {
            if (typecode.Equals(new BitString(1, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(2, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(3, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(4, 32)))
            {
                return 32;
            }
            else
            {
                return 0; // by default
            }
        }

        public static int GetPFromCode(BitString typecode)
        {
            if (typecode.Equals(new BitString(1, 32)))
            {
                return 265;
            }
            else if (typecode.Equals(new BitString(2, 32)))
            {
                return 133;
            }
            else if (typecode.Equals(new BitString(3, 32)))
            {
                return 67;
            }
            else if (typecode.Equals(new BitString(4, 32)))
            {
                return 34;
            }
            else
            {
                return 0; // by default
            }
        }
    }
}
