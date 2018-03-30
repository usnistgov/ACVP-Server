using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IAesXtsInternals
    {
        BitString MultiplyByAlpha(BitString encrypted_i, int j);
        BitString EncryptEcb(BitString key, BitString plainText);
        BitString DecryptEcb(BitString key, BitString cipherText);
    }
}
