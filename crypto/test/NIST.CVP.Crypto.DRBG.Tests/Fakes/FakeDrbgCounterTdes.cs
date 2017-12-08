using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgCounterTdes : DrbgCounterTdes
    {
        public FakeDrbgCounterTdes(IEntropyProvider entropyProvider, ITDES_ECB tdesEcb, DrbgParameters drbgParameters) :
            base(entropyProvider, tdesEcb, drbgParameters)
        {
            TdesEcb = tdesEcb;
        }

        public void PublicBlockEncrypt(BitString k, BitString x)
        {
            BlockEncrypt(k, x);
        }

        public BitString Convert168BitKeyTo192BitKey_public(BitString key)
        {
            return Convert168BitKeyTo192BitKey(key);
        }
    }
}
