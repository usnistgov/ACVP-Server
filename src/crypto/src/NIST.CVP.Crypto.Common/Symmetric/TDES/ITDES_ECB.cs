using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITDES_ECB
    {
        SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, bool encryptUsingInverseCipher = false);
        SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, bool encryptUsingInverseCipher = false);
    }
}