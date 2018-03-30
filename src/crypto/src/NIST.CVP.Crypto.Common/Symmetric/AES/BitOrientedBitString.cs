using System.Collections;
using System.Linq;
using System.Numerics;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public class BitOrientedBitString : BitString
    {
        public BitOrientedBitString(int bitCount) : base(bitCount)
        {
        }

        public BitOrientedBitString(byte[] msBytes) : base(msBytes)
        {
        }

        public BitOrientedBitString(BitArray bits) : base(bits)
        {
        }

        public BitOrientedBitString(BigInteger bigInt, int bitLength = 0) : base(bigInt, bitLength)
        {
        }

        public BitOrientedBitString(string hexMSB) : base(hexMSB)
        {
        }

        public override string ToString()
        {
            var baseToString = base.ToString();

            return baseToString.Replace(" ", string.Empty);
        }

        public static BitOrientedBitString GetBitStringEachCharacterOfInputIsBit(string bitInputInMsb)
        {
            var bitArray = MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(new string(bitInputInMsb.Reverse().ToArray()));
            return new BitOrientedBitString(bitArray);
        }

        public static BitOrientedBitString GetDerivedFromBase(BitString original)
        {
            if (original == null)
            {
                return null;
            }

            return new BitOrientedBitString(original.Bits);
        }

        public override bool Equals(object obj)
        {
            var otherBitString = obj as BitOrientedBitString;
            if (otherBitString == null)
            {
                return false;
            }

            if (this.BitLength != otherBitString.BitLength)
            {
                return false;
            }

            var copiedBits = new BitArray(this.Bits);
            var comparison = copiedBits.Xor(otherBitString.Bits);

            foreach (bool val in comparison)
            {
                if (val)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
