using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KeyWrap
{
    public abstract class KeyWrapBase : IKeyWrap
    {
        protected abstract BitString Wrap(BitString key, BitString s, bool wrapWithInverseCipher);
        protected abstract BitString WrapInverse(BitString key, BitString c, bool wrappedWithInverseCipher);
        public abstract KeyWrapResult Encrypt(BitString key, BitString plainText, bool wrapWithInverseCipher);
        public abstract KeyWrapResult Decrypt(BitString key, BitString cipherText, bool wrappedWithInverseCipher);
    }
}