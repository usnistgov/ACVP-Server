using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAesXts
    {
        SymmetricCipherResult Encrypt(XtsKey key, BitString plainText, BitString i);
        SymmetricCipherResult Decrypt(XtsKey key, BitString cipherText, BitString i);

        BitString GetIFromInteger(int dataUnitSeqNumber);
    }
}
