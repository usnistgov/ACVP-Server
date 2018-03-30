using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class SignerFactory : ISignerFactory
    {
        private HashFunction _hashFunction = new HashFunction { Mode = ModeValues.NONE, DigestSize = DigestSizes.NONE };
        private int _saltLen = 0;

        public ISignerBase GetSigner(string type)
        {
            switch (type)
            {
                case "ansx9.31":
                    return new ANS_X931_Signer(_hashFunction);

                case "pkcs1v15":
                    return new RSASSA_PKCSv15_Signer(_hashFunction);

                case "pss":
                    return new RSASSA_PSS_Signer(_hashFunction, EntropyProviderTypes.Testable, _saltLen);

                default:
                    return null;
            }
        }
    }
}
