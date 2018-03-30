using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_CBC
    {
        SymmetricCipherResult BlockEncrypt(BitString iv, BitString keyBits, BitString data);
        SymmetricCipherResult BlockDecrypt(BitString iv, BitString keyBits, BitString cipherText);
    }
}
