using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Math.Helpers
{
    public static class IntegerArrayExtensions
    {
        //@@@does endian come into play?
        public static int GetKeyBit(this int[] intArray, int bnum)
        {
            int b = intArray[bnum / 8];
            int shift = 0x08 - (bnum % 0x08);
            return 0x01 & (b >> shift);
        }
    }
}
