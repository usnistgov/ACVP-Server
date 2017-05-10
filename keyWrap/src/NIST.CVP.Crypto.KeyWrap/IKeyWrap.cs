using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    /// <summary>
    /// Describes the functions available for a KeyWrapping algorithm.
    /// </summary>
    public interface IKeyWrap
    {
        KeyWrapResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher);
        KeyWrapResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher);
    }
}