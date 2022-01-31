using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Helpers
{
    public class LmsModeMapping
    {
        public static (int m, int h, BitString typecode) GetValsFromType(LmsType type)
        {
            if (type == LmsType.LMS_SHA256_M32_H5)
            {
                return (32, 5, new BitString(5, 32));
            }
            else if (type == LmsType.LMS_SHA256_M32_H10)
            {
                return (32, 10, new BitString(6, 32));
            }
            else if (type == LmsType.LMS_SHA256_M32_H15)
            {
                return (32, 15, new BitString(7, 32));
            }
            else if (type == LmsType.LMS_SHA256_M32_H20)
            {
                return (32, 20, new BitString(8, 32));
            }
            else if (type == LmsType.LMS_SHA256_M32_H25)
            {
                return (32, 25, new BitString(9, 32));
            }
            else
            {
                return (0, 0, null);
            }
        }

        public static int GetMFromCode(BitString typecode)
        {
            if (typecode.Equals(new BitString(5, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(6, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(7, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(8, 32)))
            {
                return 32;
            }
            else if (typecode.Equals(new BitString(9, 32)))
            {
                return 32;
            }
            else
            {
                return 0; // by default
            }
        }

        public static int GetHFromCode(BitString typecode)
        {
            if (typecode.Equals(new BitString(5, 32)))
            {
                return 5;
            }
            else if (typecode.Equals(new BitString(6, 32)))
            {
                return 10;
            }
            else if (typecode.Equals(new BitString(7, 32)))
            {
                return 15;
            }
            else if (typecode.Equals(new BitString(8, 32)))
            {
                return 20;
            }
            else if (typecode.Equals(new BitString(9, 32)))
            {
                return 25;
            }
            else
            {
                return 0; // by default
            }
        }
    }
}
