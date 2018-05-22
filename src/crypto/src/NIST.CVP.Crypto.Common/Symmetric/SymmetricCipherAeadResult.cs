using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCipherAeadResult : SymmetricCipherResult
    {
        public BitString Tag { get; }

        public SymmetricCipherAeadResult(BitString cipherText, BitString tag) : base(cipherText)
        {
            Tag = tag;
        }

        public SymmetricCipherAeadResult(BitString plainText) : base(plainText) { }

        public SymmetricCipherAeadResult(string errorMessage) : base (errorMessage) { }

    }
}
