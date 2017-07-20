using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CFB128
{
    public interface IAES_CFB128
    {
        EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString plainText);
        DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
