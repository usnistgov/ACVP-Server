using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Blake2.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.Blake2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
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
        public int OutputLength => HashFunction.DigestLength;

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            if (outLen != 0 && outLen != OutputLength)
            {
                return new HashResult("Requested output length must match the configured BLAKE2b digest length.");
            }

            return ComputeHash(message, null);
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            if (key == null)
            {
                return new MacResult("Key cannot be null.");
            }

            if (macLength < 0 || macLength > OutputLength)
            {
                return new MacResult($"MAC length must be between 0 and {OutputLength} bits.");
            }

            var result = ComputeHash(message, key);
            if (!result.Success)
            {
                return new MacResult(result.ErrorMessage);
            }

            if (macLength == 0 || macLength == OutputLength)
            {
                return new MacResult(result.Digest);
            }

            return new MacResult(result.Digest.GetMostSignificantBits(macLength));
        }

        private HashResult ComputeHash(BitString message, BitString key)
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

            if (key != null && key.BitLength > 512)
            {
                return new HashResult("BLAKE2b keys must be at most 512 bits.");
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
