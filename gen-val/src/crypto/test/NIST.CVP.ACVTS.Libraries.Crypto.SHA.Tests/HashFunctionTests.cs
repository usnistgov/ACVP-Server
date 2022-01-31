using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.Tests
{
    [TestFixture]
    [FastCryptoTest]
    public class HashFunctionTests
    {
        private HashFunction _subject;

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 160, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 224, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 256, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 384, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 512, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 224, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 256, 1024)]
        [TestCase(ModeValues.SHA3, DigestSizes.d224, 224, 1152)]
        [TestCase(ModeValues.SHA3, DigestSizes.d256, 256, 1088)]
        [TestCase(ModeValues.SHA3, DigestSizes.d384, 384, 832)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512, 512, 576)]
        public void ShouldReturnCorrectOutputLenAndBlockSize(ModeValues mode, DigestSizes digestSize, int outputLen,
            int blockSize)
        {
            _subject = new HashFunction(mode, digestSize);

            Assert.AreEqual(outputLen, _subject.OutputLen);
            Assert.AreEqual(blockSize, _subject.BlockSize);
        }

        [TestCase(ModeValues.SHA1, DigestSizes.d224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d256)]
        [TestCase(ModeValues.SHA1, DigestSizes.d384)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t256)]
        [TestCase(ModeValues.SHA2, DigestSizes.d160)]
        [TestCase(ModeValues.SHA3, DigestSizes.d160)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512t256)]
        public void ShouldThrowWithInvalidModeDigestCombination(ModeValues mode, DigestSizes digestSize)
        {
            Assert.Throws(typeof(ArgumentException), () => _subject = new HashFunction(mode, digestSize));
        }
    }
}
