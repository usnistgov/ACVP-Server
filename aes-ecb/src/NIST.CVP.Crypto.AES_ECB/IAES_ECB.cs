using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_ECB
{
    public interface IAES_ECB
    {
        EncryptionResult BlockEncrypt(BitString keyBits, BitString data, bool encryptUsingInverseCipher = false);
        DecryptionResult BlockDecrypt(BitString keyBits, BitString cipherText, bool encryptedUsingInverseCipher = false);
    }
}
