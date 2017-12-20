using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class Sha3Wrapper : ISha
    {
        private readonly ISHA3Factory _iSha3Factory;
        private readonly SHA3.HashFunction _mappedHashFunction;
        private
            List<(string algo, SHAWrapper.ModeValues wrapperMode, SHAWrapper.DigestSizes wrapperSizes, int mappedDigestSize)> Sha3Mappings =
                new List<(string algo, SHAWrapper.ModeValues wrapperMode, SHAWrapper.DigestSizes wrapperSizes, int mappedDigestSize)>()
                {
                    ("SHA3-224", ModeValues.SHA3, DigestSizes.d224, 224),
                    ("SHA3-256", ModeValues.SHA3, DigestSizes.d256, 256),
                    ("SHA3-384", ModeValues.SHA3, DigestSizes.d384, 384),
                    ("SHA3-512", ModeValues.SHA3, DigestSizes.d512, 512),
                };
        
        public HashFunction HashFunction { get; }

        public Sha3Wrapper(ISHA3Factory iSha3Factory, HashFunction hashFunction)
        {
            _iSha3Factory = iSha3Factory;
            _mappedHashFunction = ToSha3HashFunction(hashFunction);
            HashFunction = hashFunction;
        }
        
        public HashResult HashMessage(BitString message)
        {
            try
            {
                var sha = _iSha3Factory.GetSHA(_mappedHashFunction);
                return new HashResult(
                    sha.HashMessage(
                        message,
                        _mappedHashFunction.DigestSize,
                        _mappedHashFunction.Capacity,
                        _mappedHashFunction.XOF
                    )
                );
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

        private SHA3.HashFunction ToSha3HashFunction(HashFunction wrapperHashFunction)
        {
            if (!Sha3Mappings
                .TryFirst(
                    w => w.wrapperMode == wrapperHashFunction.Mode && w.wrapperSizes == wrapperHashFunction.DigestSize,
                    out var result))
            {
                throw new ArgumentException($"Invalid {nameof(wrapperHashFunction)}.");
            }

            SHA3.HashFunction hashFunction = new SHA3.HashFunction()
            {
                DigestSize = result.mappedDigestSize,
                Capacity = result.mappedDigestSize * 2,
                XOF = false
            };

            return hashFunction;
        }
    }
}
