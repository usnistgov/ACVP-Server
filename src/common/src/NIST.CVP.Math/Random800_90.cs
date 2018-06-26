using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Math
{
    public class Random800_90 : IRandom800_90
    {
        private readonly Random _randy = new Random();
        public Random800_90()
        {
        }

        public virtual BitString GetRandomBitString(int length)
        {
            if (length <= 0)
            {
                return new BitString(0);
            }
            
            int numBytes = (length + 7)/8;

            var randomBytes = new byte[numBytes];
            _randy.NextBytes(randomBytes);
            var bitArray = new BitArray(randomBytes);
            bitArray.Length = length;//@@@not sure I like this because it kills the right most values

            return new BitString(bitArray);
        }

        public BitString GetDifferentBitStringOfSameSize(BitString current)
        {
            if (current == null || current.BitLength == 0)
            {
                return null;
            }
            BitString newBitString = GetRandomBitString(current.BitLength);
            while (current.Equals(newBitString))
            {
                newBitString = GetRandomBitString(current.BitLength);
            };
            return newBitString;
        }

        public int GetRandomInt(int minInclusive, int maxExclusive)
        {
            return _randy.Next(minInclusive, maxExclusive);
        }

        public BigInteger GetRandomBigInteger(BigInteger maxInclusive)
        {
            byte[] bytes = maxInclusive.ToByteArray();
            BigInteger R;

            do
            {
                _randy.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
                R = new BigInteger(bytes);
            } while (R >= maxInclusive);

            return R;
        }

        public BigInteger GetRandomBigInteger(BigInteger minInclusive, BigInteger maxInclusive)
        {
            return GetRandomBigInteger(maxInclusive - minInclusive) + minInclusive;
        }

        private const string VALID_ALPHA_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GetRandomAlphaCharacters(int length)
        {
            var result = "";
            for (var i = 0; i < length; i++)
            {
                result += VALID_ALPHA_CHARACTERS[_randy.Next(VALID_ALPHA_CHARACTERS.Length)];
            }

            return result;
        }

        private const string VALID_STRING_CHARACTERS = " !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        public string GetRandomString(int length)
        {
            var result = "";
            for (var i = 0; i < length; i++)
            {
                result += VALID_STRING_CHARACTERS[_randy.Next(VALID_STRING_CHARACTERS.Length)];
            }

            return result;
        }
    }
}
