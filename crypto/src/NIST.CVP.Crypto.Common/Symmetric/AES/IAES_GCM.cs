using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_GCM
    {
        SymmetricCipherAeadResult BlockEncrypt(BitString keyBits, BitString data, BitString iv, BitString aad, int tagLength);
        SymmetricCipherResult BlockDecrypt(BitString keyBits, BitString cipherText, BitString iv, BitString aad, BitString tag);
    }
}
