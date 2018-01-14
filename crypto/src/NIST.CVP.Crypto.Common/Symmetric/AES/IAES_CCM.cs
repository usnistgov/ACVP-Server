using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAES_CCM
    {
        SymmetricCipherResult Encrypt(BitString key, BitString nonce, BitString payload, BitString associatedData, int tagLength);
        SymmetricCipherResult Decrypt(BitString key, BitString nonce, BitString cipherText, BitString associatedData, int tagLength);
    }
}