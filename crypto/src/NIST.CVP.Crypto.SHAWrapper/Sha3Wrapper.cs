using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core.ExtensionMethods;
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
