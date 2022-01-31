using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric
{
    public class SymmetricCipherAeadResult : SymmetricCipherResult
    {
        public BitString Tag { get; }
        public bool TestPassed { get; }

        public SymmetricCipherAeadResult(BitString cipherText, BitString tag) : base(cipherText)
        {
            Tag = tag;
        }

        public SymmetricCipherAeadResult(BitString plainText) : base(plainText)
        {
            TestPassed = true;
        }

        public SymmetricCipherAeadResult(string errorMessage) : base(errorMessage) { }

        public SymmetricCipherAeadResult(BitString newCipherText, bool testPassed) : base(newCipherText)
        {
            TestPassed = testPassed;
        }
    }
}
