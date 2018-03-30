using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB1.Tests
{
    [TestFixture,  FastCryptoTest]
    public class AES_OFB1_MCTTests
    {
        private Mock<IAES_CFB1> _algo;
        private AES_CFB1_MCT _subject;

        [SetUp]
        public void Setup()
        {
            _algo = new Mock<IAES_CFB1>();
            _subject = new AES_CFB1_MCT(_algo.Object);
        }

        #region Encrypt
        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldRunEncryptOperation100000TimesForTestCase(int keySize)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString plainText = new BitString(1);
            _algo
                .Setup(s => s.BlockEncrypt(iv, key, plainText))
                .Returns(new SymmetricCipherResult(new BitOrientedBitString(1)));

            var result = _subject.MCTEncrypt(iv, key, plainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            _algo.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()), Times.Exactly(100000), nameof(_algo.Object.BlockEncrypt));
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnEncrypResponseWith100Count(int keySize)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString plainText = new BitString(1);
            _algo
                .Setup(s => s.BlockEncrypt(iv, key, plainText))
                .Returns(new SymmetricCipherResult(new BitOrientedBitString(1)));

            var result = _subject.MCTEncrypt(iv, key, plainText);

            Assert.AreEqual(100, result.Response.Count);
        }

        [Test]
        public void ShouldReturnErrorMessageOnErrorEncrypt()
        {
            string error = "Algo failure!";

            BitString iv = new BitString(128);
            BitString key = new BitString(128);
            BitString plainText = new BitString(1);
            _algo
                .Setup(s => s.BlockEncrypt(iv, key, plainText))
                .Throws(new Exception(error));

            var result = _subject.MCTEncrypt(iv, key, plainText);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(error, result.ErrorMessage, nameof(result.ErrorMessage));
        }
        #endregion Encrypt

        #region Decrypt
        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldRunDecryptOperation100000TimesForTestCase(int keySize)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString cipherText = new BitString(1);
            _algo
                .Setup(s => s.BlockDecrypt(iv, key, cipherText))
                .Returns(new SymmetricCipherResult(new BitOrientedBitString(1)));

            var result = _subject.MCTDecrypt(iv, key, cipherText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            _algo.Verify(v => v.BlockDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()), Times.Exactly(100000), nameof(_algo.Object.BlockDecrypt));
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnDecrypResponseWith100Count(int keySize)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString cipherText = new BitString(1);
            _algo
                .Setup(s => s.BlockDecrypt(iv, key, cipherText))
                .Returns(new SymmetricCipherResult(new BitOrientedBitString(1)));

            var result = _subject.MCTDecrypt(iv, key, cipherText);

            Assert.AreEqual(100, result.Response.Count);
        }

        [Test]
        public void ShouldReturnErrorMessageOnErrorDecrypt()
        {
            string error = "Algo failure!";

            BitString iv = new BitString(128);
            BitString key = new BitString(128);
            BitString cipherText = new BitString(1);
            _algo
                .Setup(s => s.BlockDecrypt(iv, key, cipherText))
                .Throws(new Exception(error));

            var result = _subject.MCTDecrypt(iv, key, cipherText);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(error, result.ErrorMessage, nameof(result.ErrorMessage));
        }
        #endregion Decrypt
    }
}
