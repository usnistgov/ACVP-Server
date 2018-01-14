using System;
using System.Collections;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgCounterTdes : DrbgCounterBase
    {
        protected ITDES_ECB TdesEcb;

        public DrbgCounterTdes(IEntropyProvider entropyProvider, ITDES_ECB tdesEcb, DrbgParameters drbgParameters)
            : base(entropyProvider, drbgParameters)
        {
            TdesEcb = tdesEcb;
        }

        protected override BitString BlockEncrypt(BitString K, BitString X)
        {
            return TdesEcb.BlockEncrypt(Convert168BitKeyTo192BitKey(K), X).Result;
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
