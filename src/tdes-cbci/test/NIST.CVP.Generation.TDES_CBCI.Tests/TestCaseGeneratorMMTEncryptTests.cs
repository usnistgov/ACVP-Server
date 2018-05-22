using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
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
                .Returns(new BitString(192));
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _mode = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new SymmetricCipherResult(new BitString(64)));
            _modeFactory = new Mock<IModeBlockCipherFactory>();
            _modeFactory
                .Setup(s => s.GetStandardCipher(
                    It.IsAny<IBlockCipherEngine>(),
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mode.Object);
            _subject = new TestCaseGeneratorMMTEncrypt(
                _random.Object,
                _engineFactory.Object,
                _modeFactory.Object
            );
        }

        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var result = _subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldHaveProperNumberOfTestCasesToGenerate()
        {
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldReturnAnErrorIfAnEncryptionFails()
        {
            _mode.Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
               .Returns(new SymmetricCipherResult("I Failed to encrypt"));
            var result = _subject.Generate(new TestGroup { Function = "encrypt", KeyingOption = 1 }, false);
            Assert.IsFalse(result.Success);
        }
    }
}
