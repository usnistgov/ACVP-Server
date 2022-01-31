using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents
{
    public class HashConditioningComponentFactory : IHashConditioningComponentFactory
    {
        private readonly IDrbgFactory _factory;

        public HashConditioningComponentFactory(IDrbgFactory factory)
        {
            _factory = factory;
        }

        public IDrbgConditioningComponent GetInstance(HashFunction hashFunction)
        {
            var drbgParameters = new DrbgParameters
            {
                Mechanism = DrbgMechanism.Hash,
                Mode = GetModeFromHashFunction(hashFunction)
            };

            var drbg = _factory.GetDrbgInstance(drbgParameters, new EntropyProvider(new Random800_90()));

            return new HashConditioningComponent((DrbgHash)drbg);
        }

        private DrbgMode GetModeFromHashFunction(HashFunction hashFunction)
        {
            return hashFunction.Mode switch
            {
                ModeValues.SHA1 => DrbgMode.SHA1,
                ModeValues.SHA2 => hashFunction.DigestSize switch
                {
                    DigestSizes.d224 => DrbgMode.SHA224,
                    DigestSizes.d256 => DrbgMode.SHA256,
                    DigestSizes.d384 => DrbgMode.SHA384,
                    DigestSizes.d512 => DrbgMode.SHA512,
                    DigestSizes.d512t224 => DrbgMode.SHA512t224,
                    DigestSizes.d512t256 => DrbgMode.SHA512t256,
                    _ => throw new ArgumentException("Unsupported hash function")
                },
                _ => throw new ArgumentException("Unsupported hash function")
            };
        }
    }
}
