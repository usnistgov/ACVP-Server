using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Core
{
    public interface IModeOfOperation
    {
        TDES_CFB.Algo Algo { get; }

        EncryptionResult BlockEncrypt(BitString key, BitString iv, BitString plainText);
        DecryptionResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}
