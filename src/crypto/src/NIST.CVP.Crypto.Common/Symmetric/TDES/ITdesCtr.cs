using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITdesCtr
    {
        SymmetricCipherResult EncryptBlock(BitString key, BitString plainText, BitString iv);
        SymmetricCipherResult DecryptBlock(BitString key, BitString cipherText, BitString iv);
        SymmetricCounterResult Encrypt(BitString key, BitString plainText, ICounter counter);
        SymmetricCounterResult Decrypt(BitString key, BitString cipherText, ICounter counter);
    }
}
