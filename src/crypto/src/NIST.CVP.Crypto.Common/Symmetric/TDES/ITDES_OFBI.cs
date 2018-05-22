using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ITDES_OFBI
    {
        SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText);
        SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}