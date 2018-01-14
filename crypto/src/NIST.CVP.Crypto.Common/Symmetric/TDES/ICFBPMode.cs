using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ICFBPMode
    {
        Algo Algo { get; set; }
        SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText, bool includeAuxValues = false);
        SymmetricCipherWithIvResult BlockDecrypt(BitString key, BitString iv, BitString cipherText1, BitString cipherText2, BitString cipherText3);
        SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText, bool includeAuxValues = false);
        SymmetricCipherWithIvResult BlockEncrypt(BitString key, BitString iv, BitString plainText1, BitString plainText2, BitString plainText3);
    }
}
