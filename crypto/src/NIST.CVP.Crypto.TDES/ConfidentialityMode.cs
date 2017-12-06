using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public abstract class ConfidentialityMode : IModeOfOperation
    {
        public Algo Algo { get; protected set; }
        //protected BlockCipher BlockCipher { get; set; }
        public abstract EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText, bool produceAuxiliaryValues = false);
        public abstract DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText, bool produceAuxiliaryValues = false);



    }
}
