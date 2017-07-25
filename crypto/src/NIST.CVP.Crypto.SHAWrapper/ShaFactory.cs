using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class ShaFactory : IShaFactory
    {
        public ISha GetShaInstance(HashFunction hashFunction)
        {
            switch (hashFunction.Mode)
            {
                case ModeValues.SHA1:
                case ModeValues.SHA2:
                    var mapped2HashFunction = ToSha2HashFunction(hashFunction);
                    return new Sha2Wrapper(new SHAFactory(), mapped2HashFunction);
                case ModeValues.SHA3:
                    var mapped3HashFunction = ToSha3HashFunction(hashFunction);
                    return new Sha3Wrapper(new SHA3Factory(), mapped3HashFunction);
                default:
                    throw new ArgumentException($"{nameof(hashFunction)}");
            }
        }

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

        private
            List<(string algo, SHAWrapper.ModeValues wrapperMode, SHAWrapper.DigestSizes wrapperSizes, int mappedDigestSize)> Sha3Mappings =
                new List<(string algo, SHAWrapper.ModeValues wrapperMode, SHAWrapper.DigestSizes wrapperSizes, int mappedDigestSize)>()
                {
                    ("SHA3-224", ModeValues.SHA3, DigestSizes.d224, 224),
                    ("SHA3-256", ModeValues.SHA3, DigestSizes.d256, 256),
                    ("SHA3-384", ModeValues.SHA3, DigestSizes.d384, 384),
                    ("SHA3-512", ModeValues.SHA3, DigestSizes.d512, 512),
                };

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
