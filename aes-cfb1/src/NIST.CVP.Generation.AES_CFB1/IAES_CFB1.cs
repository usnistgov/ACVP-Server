using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1
{
    public interface IAES_CFB1
    {
        EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString plainText);
        DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
