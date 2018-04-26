using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class Sha2Wrapper : ISha
    {
        private readonly ISHAFactory _shaFactory;
        private readonly Common.Hash.SHA2.HashFunction _mappedHashFunction;
        private
            List<(string algo, Common.Hash.ShaWrapper.ModeValues wrapperMode, Common.Hash.SHA2.ModeValues mappedMode, Common.Hash.ShaWrapper.DigestSizes wrapperSizes, Common.Hash.SHA2.DigestSizes mappedDigestSize)> Sha2Mappings =
                new List<(string algo, Common.Hash.ShaWrapper.ModeValues wrapperMode, Common.Hash.SHA2.ModeValues mappedMode, Common.Hash.ShaWrapper.DigestSizes wrapperSizes, Common.Hash.SHA2.DigestSizes mappedDigestSize)>()
                {
                    ("SHA1", Common.Hash.ShaWrapper.ModeValues.SHA1, Common.Hash.SHA2.ModeValues.SHA1, Common.Hash.ShaWrapper.DigestSizes.d160, Common.Hash.SHA2.DigestSizes.d160),
                    ("SHA2-224", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d224, Common.Hash.SHA2.DigestSizes.d224),
                    ("SHA2-256", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d256, Common.Hash.SHA2.DigestSizes.d256),
                    ("SHA2-384", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d384, Common.Hash.SHA2.DigestSizes.d384),
                    ("SHA2-512", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d512, Common.Hash.SHA2.DigestSizes.d512),
                    ("SHA2-512/224", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d512t224, Common.Hash.SHA2.DigestSizes.d512t224),
                    ("SHA2-512/256", Common.Hash.ShaWrapper.ModeValues.SHA2, Common.Hash.SHA2.ModeValues.SHA2, Common.Hash.ShaWrapper.DigestSizes.d512t256, Common.Hash.SHA2.DigestSizes.d512t256),
                };

        public Common.Hash.ShaWrapper.HashFunction HashFunction { get; }
        
        public Sha2Wrapper(ISHAFactory shaFactory, Common.Hash.ShaWrapper.HashFunction hashFunction)
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

        private Common.Hash.SHA2.HashFunction ToSha2HashFunction(Common.Hash.ShaWrapper.HashFunction wrapperHashFunction)
        {
            if (!Sha2Mappings
                .TryFirst(
                    w => w.wrapperMode == wrapperHashFunction.Mode && w.wrapperSizes == wrapperHashFunction.DigestSize,
                    out var result))
            {
                throw new ArgumentException($"Invalid {nameof(wrapperHashFunction)}.");
            }

            Common.Hash.SHA2.HashFunction hashFunction = new Common.Hash.SHA2.HashFunction()
            {
                DigestSize = result.mappedDigestSize,
                Mode = result.mappedMode
            };

            return hashFunction;
        }
    }
}
