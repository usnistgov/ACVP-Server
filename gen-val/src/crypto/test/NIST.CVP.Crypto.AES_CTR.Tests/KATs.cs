﻿using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.AES.KATs;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class KATs
    {
        private static int[] _keyLens = {128, 192, 256};

        private Mock<ICounter> _mockCounter;
        private CtrBlockCipher _subject;

        [SetUp]
        public void Setup()
        {
            _mockCounter = new Mock<ICounter>();
            _subject = new CtrBlockCipher(new AesEngine(), _mockCounter.Object);
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldGfSBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetGfSBox(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);

                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldKeySBoxCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetKeySBox(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);
                
                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarKeyCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarKey(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);
                
                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }

        [Test]
        [TestCaseSource(nameof(_keyLens))]
        public void ShouldVarTxtCorrectlyWithEachKeySize(int keyLen)
        {
            foreach (var test in KatDataCtr.GetVarTxt(keyLen))
            {
                _mockCounter.Setup(s => s.GetNextIV()).Returns(test.IV);

                var param = new ModeBlockCipherParameters(
                    BlockCipherDirections.Encrypt,
                    test.Key,
                    test.PlainText
                );
                var result = _subject.ProcessPayload(param);
                
                Assert.IsTrue(result.Success, nameof(result.Success));
                Assert.AreEqual(test.CipherText, result.Result, test.CipherText.ToHex());
            }
        }
    }
}