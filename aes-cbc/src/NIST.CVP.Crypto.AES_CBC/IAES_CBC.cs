using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CBC
{
    public interface IAES_CBC
    {
        EncryptionResult BlockEncrypt(BitString iv, BitString keyBits, BitString data);
        DecryptionResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
