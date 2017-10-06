using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class DsaFfcFactory : IDsaFfcFactory
    {
        public IDsaFfc GetInstance(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new FfcDsa(sha, entropyType);
        }
    }
}
