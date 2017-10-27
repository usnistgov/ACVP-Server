using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class DsaFfcFactory : IDsaFfcFactory
    {
        private readonly IShaFactory _shaFactory;

        public DsaFfcFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IDsaFfc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new FfcDsa(_shaFactory.GetShaInstance(hashFunction), entropyType);
        }
    }
}
