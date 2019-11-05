using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccNonceProviderFactory : IEccNonceProviderFactory
    {
        public IEccNonceProvider GetNonceProvider(NonceProviderTypes nonceTypes, IHmac hmac, IEntropyProvider entropyProvider)
        {
            switch (nonceTypes)
            {
                case NonceProviderTypes.Deterministic:
                    return new DeterministicNonceProvider(hmac);
                
                case NonceProviderTypes.Random:
                    return new RandomNonceProvider(entropyProvider);
            }
            
            throw new ArgumentException("Invalid nonce type provided");
        }
    }
}