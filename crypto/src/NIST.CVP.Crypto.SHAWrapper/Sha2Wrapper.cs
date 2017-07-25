using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core.ExtensionMethods;
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
