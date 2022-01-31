using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap
{
    /// <summary>
    /// Describes the functions available for a KeyWrapping algorithm.
    /// </summary>
    public interface IKeyWrap
    {
        SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher);
        SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher);
    }
}
