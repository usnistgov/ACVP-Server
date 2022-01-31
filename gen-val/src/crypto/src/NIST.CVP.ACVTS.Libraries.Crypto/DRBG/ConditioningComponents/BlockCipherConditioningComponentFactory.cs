using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents
{
    public class BlockCipherConditioningComponentFactory : IBlockCipherConditioningComponentFactory
    {
        private readonly IDrbgFactory _factory;

        public BlockCipherConditioningComponentFactory(IDrbgFactory factory)
        {
            _factory = factory;
        }

        public IDrbgConditioningComponent GetInstance(int keyLength)
        {
            var drbgParameters = new DrbgParameters
            {
                Mechanism = DrbgMechanism.Counter,
                Mode = GetModeFromKeyLength(keyLength)
            };

            var drbgAes = _factory.GetDrbgInstance(drbgParameters, new EntropyProvider(new Random800_90()));

            // Cast because we need AES specifically
            return new BlockCipherConditioningComponent((DrbgCounterAes)drbgAes);
        }

        private DrbgMode GetModeFromKeyLength(int keyLength)
        {
            return keyLength switch
            {
                128 => DrbgMode.AES128,
                192 => DrbgMode.AES192,
                256 => DrbgMode.AES256,
                _ => throw new ArgumentException("Invalid keyLength provided")
            };
        }
    }
}
