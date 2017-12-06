using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public interface IModeOfOperation
    {
        Algo Algo { get; }

        EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText, bool produceAuxiliaryValues = false);
        DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText, bool produceAuxiliaryValues = false);
    }
}
