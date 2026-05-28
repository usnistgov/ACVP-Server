using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Blake2.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Blake2
{
    public class Blake2b : IBlake2
    {
        public Blake2b(Blake2HashFunction hashFunction)
        {
            if (hashFunction.Variant != Blake2Variant.Blake2b)
            {
                throw new ArgumentException("BLAKE2b requires the BLAKE2b variant.", nameof(hashFunction));
            }

            if (hashFunction.DigestLength > 512)
            {
                throw new ArgumentException("BLAKE2b digest length must be at most 512 bits.", nameof(hashFunction));
            }

            HashFunction = hashFunction;
        }

        public Blake2HashFunction HashFunction { get; }

        public HashResult HashMessage(BitString message, BitString key = null)
        {
            if (message == null)
            {
                return new HashResult("Message cannot be null.");
            }

            if (message.BitLength % 8 != 0)
            {
                return new HashResult("BLAKE2b currently supports byte-aligned messages only.");
            }

            if (key != null && key.BitLength % 8 != 0)
            {
                return new HashResult("BLAKE2b currently supports byte-aligned keys only.");
            }

            var config = new Blake2BConfig
            {
                OutputSizeInBits = HashFunction.DigestLength,
                Key = key?.GetPaddedBytes()
            };

            var digest = Blake2B.ComputeHash(message.GetPaddedBytes(), config);
            return new HashResult(new BitString(digest));
        }
    }
}
