using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap
{
    public abstract class KeyWrapBase : IKeyWrap
    {
        protected abstract BitString Wrap(BitString key, BitString s, bool wrapWithInverseCipher);
        protected abstract BitString WrapInverse(BitString key, BitString c, bool wrappedWithInverseCipher);
        public abstract SymmetricCipherResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher);
        public abstract SymmetricCipherResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher);
    }
}
