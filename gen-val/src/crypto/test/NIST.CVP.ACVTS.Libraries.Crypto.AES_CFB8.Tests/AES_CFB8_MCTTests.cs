﻿using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CFB8.Tests
{
    [TestFixture, FastCryptoTest]
    public class AES_CFB8_MCTTests
    {
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _algo;
        private Mock<IModeBlockCipherFactory> _modeFactory;
        private Mock<IMonteCarloKeyMakerAes> _keyMaker;
        private MonteCarloAesCfb _subject;

        [SetUp]
        public void Setup()
        {
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _algo = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
            _algo
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new SymmetricCipherResult(new BitString(128)));
            _modeFactory = new Mock<IModeBlockCipherFactory>();
            _modeFactory
                .Setup(s => s.GetStandardCipher(_engine.Object, It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(_algo.Object);
            _keyMaker = new Mock<IMonteCarloKeyMakerAes>();
            _keyMaker
                .Setup(s => s.MixKeys(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(() => new BitString(128));

            _subject = new MonteCarloAesCfb(
                _engineFactory.Object,
                _modeFactory.Object,
                _keyMaker.Object,
                8,
                BlockCipherModesOfOperation.CfbByte
            );
        }

        [Test]
        [TestCase(128, BlockCipherDirections.Encrypt)]
        [TestCase(192, BlockCipherDirections.Encrypt)]
        [TestCase(256, BlockCipherDirections.Encrypt)]
        [TestCase(128, BlockCipherDirections.Decrypt)]
        [TestCase(192, BlockCipherDirections.Decrypt)]
        [TestCase(256, BlockCipherDirections.Decrypt)]
        public void ShouldRunEncryptOperation100000TimesForTestCase(int keySize, BlockCipherDirections direction)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString payload = new BitString(128);

            var p = new ModeBlockCipherParameters(
                direction,
                iv,
                key,
                payload
            );

            var result = _subject.ProcessMonteCarloTest(p);

            Assert.That(result.Success, Is.True, nameof(result.Success));
            _algo.Verify(v => v.ProcessPayload(
                It.IsAny<ModeBlockCipherParameters>()),
                Times.Exactly(100000),
                nameof(_algo.Object.ProcessPayload)
            );
        }

        [Test]
        [TestCase(128, BlockCipherDirections.Encrypt)]
        [TestCase(192, BlockCipherDirections.Encrypt)]
        [TestCase(256, BlockCipherDirections.Encrypt)]
        [TestCase(128, BlockCipherDirections.Decrypt)]
        [TestCase(192, BlockCipherDirections.Decrypt)]
        [TestCase(256, BlockCipherDirections.Decrypt)]
        public void ShouldReturnEncrypResponseWith100Count(int keySize, BlockCipherDirections direction)
        {
            BitString iv = new BitString(128);
            BitString key = new BitString(keySize);
            BitString payload = new BitString(128);

            var p = new ModeBlockCipherParameters(
                direction,
                iv,
                key,
                payload
            );

            var result = _subject.ProcessMonteCarloTest(p);

            Assert.That(result.Response.Count, Is.EqualTo(100));
        }

        [Test]
        [TestCase(BlockCipherDirections.Encrypt)]
        [TestCase(BlockCipherDirections.Decrypt)]
        public void ShouldReturnErrorMessageOnErrorEncrypt(BlockCipherDirections direction)
        {
            string error = "Algo failure!";

            BitString iv = new BitString(128);
            BitString key = new BitString(128);
            BitString payload = new BitString(128);
            _algo
                .Setup(s => s.ProcessPayload(It.IsAny<ModeBlockCipherParameters>()))
                .Throws(new Exception(error));

            var p = new ModeBlockCipherParameters(
                direction,
                iv,
                key,
                payload
            );

            var result = _subject.ProcessMonteCarloTest(p);

            Assert.That(result.Success, Is.False, nameof(result.Success));
            Assert.That(result.ErrorMessage, Is.EqualTo(error), nameof(result.ErrorMessage));
        }
    }
}
