using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_CFB1
    {
        SymmetricCipherResult BlockEncrypt(BitString iv, BitString keyBits, BitString plainText);
        SymmetricCipherResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
