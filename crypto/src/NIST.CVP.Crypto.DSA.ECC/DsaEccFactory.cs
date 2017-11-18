using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class DsaEccFactory : IDsaEccFactory
    {
        private readonly IShaFactory _shaFactory;

        public DsaEccFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IDsaEcc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new EccDsa(_shaFactory.GetShaInstance(hashFunction), entropyType);
        }
    }
}
