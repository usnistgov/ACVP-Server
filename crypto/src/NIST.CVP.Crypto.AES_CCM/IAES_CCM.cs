using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CCM
{
    public interface IAES_CCM
    {
        EncryptionResult Encrypt(BitString key, BitString nonce, BitString payload, BitString associatedData, int tagLength);
        DecryptionResult Decrypt(BitString key, BitString nonce, BitString cipherText, BitString associatedData, int tagLength);
    }
}