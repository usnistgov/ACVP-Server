using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_OFB
{
    public interface IAES_OFB
    {
        EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString plainText);
        DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
