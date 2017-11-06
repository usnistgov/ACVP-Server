using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public abstract class ConfidentialityMode : IModeOfOperation
    {
        public Algo Algo { get; protected set; }
        protected BlockCipher BlockCipher { get; set; }
        public abstract EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText);
        public abstract DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);



    }
}
