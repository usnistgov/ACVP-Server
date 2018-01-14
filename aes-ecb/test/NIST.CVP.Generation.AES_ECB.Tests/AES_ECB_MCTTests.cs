using System;
using Moq;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class AES_ECB_MCTTests
    {
        private Mock<IAES_ECB> _aesEcb;
        private AES_ECB_MCT _subject;

        [SetUp]
        public void Setup()
        {
            _aesEcb = new Mock<IAES_ECB>();
            _subject = new AES_ECB_MCT(_aesEcb.Object);
        }

        #region Encrypt
        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldRunEncryptOperation100000TimesForTestCase(int keySize)
        {
            BitString key = new BitString(keySize);
            BitString plainText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockEncrypt(key, plainText, false))
                .Returns(new SymmetricCipherResult(new BitString(128)));

            var result = _subject.MCTEncrypt(key, plainText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            _aesEcb.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false), Times.Exactly(100000), nameof(_aesEcb.Object.BlockEncrypt));
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnEncrypResponseWith100Count(int keySize)
        {
            BitString key = new BitString(keySize);
            BitString plainText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockEncrypt(key, plainText, false))
                .Returns(new SymmetricCipherResult(new BitString(128)));

            var result = _subject.MCTEncrypt(key, plainText);

            Assert.AreEqual(100, result.Response.Count);
        }

        [Test]
        public void ShouldReturnErrorMessageOnErrorEncrypt()
        {
            string error = "Algo failure!";

            BitString key = new BitString(128);
            BitString plainText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockEncrypt(key, plainText, false))
                .Throws(new Exception(error));

            var result = _subject.MCTEncrypt(key, plainText);

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
            BitString key = new BitString(keySize);
            BitString cipherText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockDecrypt(key, cipherText, false))
                .Returns(new SymmetricCipherResult(new BitString(128)));

            var result = _subject.MCTDecrypt(key, cipherText);

            Assert.IsTrue(result.Success, nameof(result.Success));
            _aesEcb.Verify(v => v.BlockDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false), Times.Exactly(100000), nameof(_aesEcb.Object.BlockDecrypt));
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        public void ShouldReturnDecrypResponseWith100Count(int keySize)
        {
            BitString key = new BitString(keySize);
            BitString cipherText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockDecrypt(key, cipherText, false))
                .Returns(new SymmetricCipherResult(new BitString(128)));

            var result = _subject.MCTDecrypt(key, cipherText);

            Assert.AreEqual(100, result.Response.Count);
        }

        [Test]
        public void ShouldReturnErrorMessageOnErrorDecrypt()
        {
            string error = "Algo failure!";

            BitString key = new BitString(128);
            BitString cipherText = new BitString(128);
            _aesEcb
                .Setup(s => s.BlockDecrypt(key, cipherText, false))
                .Throws(new Exception(error));

            var result = _subject.MCTDecrypt(key, cipherText);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(error, result.ErrorMessage, nameof(result.ErrorMessage));
        }
        #endregion Decrypt
    }
}
