using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Helpers
{
    public static class XtsHelper
    {
        public static BitString GetIFromBigInteger(BigInteger dataUnitSeqNumber)
        {
            // the tweak is a 128-bit value
            var tweakValue = new BitString(dataUnitSeqNumber, 128);
            // Before encrypting, "the tweak is first converted into a little-endian byte array." -- ref. IEEE1619-2007 section 5.1
            return BitString.ReverseByteOrder(tweakValue);
        }

        public static BigInteger GetBigIntegerFromI(BitString reverseByteOrderTweak)
        {
            // Undo the reverse byte ordering
            var tweak = BitString.ReverseByteOrder(reverseByteOrderTweak);
            return tweak.ToBigInteger();
        }
    }
}
