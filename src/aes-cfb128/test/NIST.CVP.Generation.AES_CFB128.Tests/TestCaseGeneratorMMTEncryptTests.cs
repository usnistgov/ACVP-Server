using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB128.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMMTEncryptTests
    {
        private Mock<IRandom800_90> _random;
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _mode;
        private Mock<IModeBlockCipherFactory> _modeFactory;
        private TestCaseGeneratorMMTEncrypt _subject;

        [SetUp]
        public void Setup()
        {
            _random = new Mock<IRandom800_90>();
            _random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(() => new BitString(128));
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _mode = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new SymmetricCipherResult(new BitString(128)));
            _modeFactory = new Mock<IModeBlockCipherFactory>();
            _modeFactory
                .Setup(s => s.GetStandardCipher(
                    It.IsAny<IBlockCipherEngine>(),
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mode.Object);
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var testGroup = GetTesGroup();
            _subject = new TestCaseGeneratorMMTEncrypt(testGroup, _random.Object, _engineFactory.Object, _modeFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailed()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult("Fail"));

            var testGroup = GetTesGroup();
            _subject = new TestCaseGeneratorMMTEncrypt(testGroup, _random.Object, _engineFactory.Object, _modeFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnException()
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception());

            var testGroup = GetTesGroup();
            _subject = new TestCaseGeneratorMMTEncrypt(testGroup, _random.Object, _engineFactory.Object, _modeFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeProcessPayload()
        {
            var testGroup = GetTesGroup();
            _subject = new TestCaseGeneratorMMTEncrypt(testGroup, _random.Object, _engineFactory.Object, _modeFactory.Object);
            _subject.Generate(testGroup, true);

            _mode.Verify(v => v.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()),
                Times.AtLeastOnce,
                nameof(_mode.Object.ProcessPayload)
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakePayload = new BitString(new byte[] { 1 });
            _random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult(fakePayload));

            var testGroup = GetTesGroup();
            _subject = new TestCaseGeneratorMMTEncrypt(testGroup, _random.Object, _engineFactory.Object, _modeFactory.Object);
            var result = _subject.Generate(testGroup, false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty((result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty((result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty((result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        private TestGroup GetTesGroup()
        {
            return new TestGroup()
            {
                KeyLength = 128,
                AlgoMode = Common.AlgoMode.AES_CFB128
            };
        }
    }
}
