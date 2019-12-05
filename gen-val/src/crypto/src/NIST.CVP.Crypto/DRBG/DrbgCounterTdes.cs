using System;
using System.Collections;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgCounterTdes : DrbgCounterBase
    {
        protected IModeBlockCipher<IModeBlockCipherResult> Cipher;

        public DrbgCounterTdes(IEntropyProvider entropyProvider, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, DrbgParameters drbgParameters)
            : base(entropyProvider, drbgParameters)
        {
            Cipher = cipherFactory.GetStandardCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes), BlockCipherModesOfOperation.Ecb);
        }

        protected override BitString BlockEncrypt(BitString K, BitString X)
        {
            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, Convert168BitKeyTo192BitKey(K), X);
            return Cipher.ProcessPayload(param).Result;
        }

        protected BitString Convert168BitKeyTo192BitKey(BitString origKey)
        {
            if (origKey.BitLength != 168)
            {
                throw new ArgumentException("Not a valid key, needs 168 bits");
            }

            var bsReverse = new BitString(MsbLsbConversionHelpers.ReverseBitArrayBits(origKey.Bits));
            var newBs = new BitString(0);

            for (var i = 0; i < bsReverse.BitLength; i++)
            {
                newBs = newBs.ConcatenateBits(new BitString(new BitArray(new [] { bsReverse.Bits[i] })));

                if ((i + 1) % 7 == 0)
                {
                    newBs = newBs.ConcatenateBits(BitString.Zero());
                }
            }

            return newBs;
        }
    }
}
