using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class Sha2Wrapper : ISha
    {
        private readonly ISHAFactory _shaFactory;
        private readonly SHA2.HashFunction _mappedHashFunction;
        private
            List<(string algo, SHAWrapper.ModeValues wrapperMode, SHA2.ModeValues mappedMode, SHAWrapper.DigestSizes wrapperSizes, SHA2.DigestSizes mappedDigestSize)> Sha2Mappings =
                new List<(string algo, SHAWrapper.ModeValues wrapperMode, SHA2.ModeValues mappedMode, SHAWrapper.DigestSizes wrapperSizes, SHA2.DigestSizes mappedDigestSize)>()
                {
                    ("SHA1", ModeValues.SHA1, SHA2.ModeValues.SHA1, DigestSizes.d160, SHA2.DigestSizes.d160),
                    ("SHA2-224", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d224, SHA2.DigestSizes.d224),
                    ("SHA2-256", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d256, SHA2.DigestSizes.d256),
                    ("SHA2-384", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d384, SHA2.DigestSizes.d384),
                    ("SHA2-512", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d512, SHA2.DigestSizes.d512),
                    ("SHA2-512/224", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d512t224, SHA2.DigestSizes.d512t224),
                    ("SHA2-512/256", ModeValues.SHA2, SHA2.ModeValues.SHA2, DigestSizes.d512t256, SHA2.DigestSizes.d512t256),
                };

        public HashFunction HashFunction { get; }
        
        public Sha2Wrapper(ISHAFactory shaFactory, HashFunction hashFunction)
        {
            _shaFactory = shaFactory;
            _mappedHashFunction = ToSha2HashFunction(hashFunction);
            HashFunction = hashFunction;
        }
        
        public HashResult HashMessage(BitString message)
        {
            try
            {
                var sha = _shaFactory.GetSHA(_mappedHashFunction);
                return new HashResult(sha.HashMessage(message));
            }
            catch (Exception ex)
            {
                return new HashResult(ex.Message);
            }
        }

        public HashResult HashNumber(BigInteger number)
        {
            var bs = new BitString(number);
            
            // Pad the BitString to be a multiple of 32 bits
            // Likely a relic of old MultiPrecision libraries
            // Spec says to just hash the integer which is normally 4 bytes but 
            //      with larger integer values, libraries keep them at multiples
            //      of 4 bytes, so we have to make sure our structure is a multiple
            //      of 4 bytes as well.
            if (bs.BitLength % 32 != 0)
            {
                bs = BitString.ConcatenateBits(BitString.Zeroes(32 - bs.BitLength % 32), bs);
            }

            return HashMessage(bs);
        }

        private SHA2.HashFunction ToSha2HashFunction(HashFunction wrapperHashFunction)
        {
            if (!Sha2Mappings
                .TryFirst(
                    w => w.wrapperMode == wrapperHashFunction.Mode && w.wrapperSizes == wrapperHashFunction.DigestSize,
                    out var result))
            {
                throw new ArgumentException($"Invalid {nameof(wrapperHashFunction)}.");
            }

            SHA2.HashFunction hashFunction = new SHA2.HashFunction()
            {
                DigestSize = result.mappedDigestSize,
                Mode = result.mappedMode
            };

            return hashFunction;
        }
    }
}
