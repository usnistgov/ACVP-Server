using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.MCT;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha
{
    public class NativeShaFactory : IShaFactory
    {
        public ISha GetShaInstance(HashFunction hashFunction)
        {
            switch (hashFunction.Mode)
            {
                case ModeValues.SHA1:
                    return new NativeFastSha1();

                case ModeValues.SHA2:
                    return hashFunction.DigestSize switch
                    {
                        DigestSizes.d224 => new NativeFastSha2_224(),
                        DigestSizes.d256 => new NativeFastSha2_256(),
                        DigestSizes.d384 => new NativeFastSha2_384(),
                        DigestSizes.d512 => new NativeFastSha2_512(),
                        DigestSizes.d512t224 => new NativeFastSha2_512_224(),
                        DigestSizes.d512t256 => new NativeFastSha2_512_256(),
                        _ => throw new ArgumentException($"{nameof(hashFunction)}")
                    };

                case ModeValues.SHA3:
                    return hashFunction.DigestSize switch
                    {
                        DigestSizes.d224 => new NativeFastSha3(224),
                        DigestSizes.d256 => new NativeFastSha3(256),
                        DigestSizes.d384 => new NativeFastSha3(384),
                        DigestSizes.d512 => new NativeFastSha3(512),
                        _ => throw new ArgumentException($"{nameof(hashFunction)}")
                    };

                case ModeValues.SHAKE:
                    return hashFunction.DigestSize switch
                    {
                        DigestSizes.d128 => new NativeFastShake(128),
                        DigestSizes.d256 => new NativeFastShake(256),
                        _ => throw new ArgumentException($"{nameof(hashFunction)}")
                    };

                default:
                    throw new ArgumentException($"{nameof(hashFunction)}");
            }
        }

        public IShaMct GetShaMctInstance(HashFunction hashFunction, bool isAlternate = false)
        {
            var sha = GetShaInstance(hashFunction);
            
            switch (hashFunction.Mode)
            {
                case ModeValues.SHA1:
                case ModeValues.SHA2:
                    if (isAlternate) return new AlternateSizeShaMct(sha);
                    return new StandardSizeShaMct(sha);
                case ModeValues.SHA3:
                    if (isAlternate) return new AlternateSizeSha3Mct(sha);
                    return new StandardSizeSha3Mct(sha);
                case ModeValues.SHAKE:
                    return new ShakeMct(sha);

                default:
                    throw new ArgumentException($"{nameof(hashFunction)}");
            }
        }

        public IShake GetShakeInstance(HashFunction hashFunction)
        {
            return hashFunction.Mode switch
            {
                ModeValues.SHAKE => hashFunction.DigestSize switch
                {
                    DigestSizes.d128 => new NativeFastShake(128),
                    DigestSizes.d256 => new NativeFastShake(256),
                    _ => throw new ArgumentException("Invalid SHAKE")
                },
                _ => throw new ArgumentException("Invalid SHAKE")
            };
        }
    }
}
