using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public interface ICFBMode
    {
        Algo Algo { get; set; }
        SymmetricCipherResult BlockDecrypt(BitString key, BitString iv, BitString cipherText);
        SymmetricCipherResult BlockEncrypt(BitString key, BitString iv, BitString plainText);
    }
}
