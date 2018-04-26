using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_ECB
    {
        SymmetricCipherResult BlockEncrypt(BitString keyBits, BitString data, bool encryptUsingInverseCipher = false);
        SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, bool encryptedUsingInverseCipher = false);
    }
}
