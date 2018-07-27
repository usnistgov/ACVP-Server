using System;
using Moq;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NUnit.Framework;


namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
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
                .Returns(new BitString(64));
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
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldSuccessfullyGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMMTEncrypt(group, new Random800_90(), _engineFactory.Object, _modeFactory.Object);
            
            var result = _subject.Generate(group, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(AlgoMode algo)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMMTEncrypt(group, new Random800_90(), _engineFactory.Object, _modeFactory.Object);
            Assert.AreEqual(10, _subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1, 1)]
        [TestCase(AlgoMode.TDES_CFB8, 8)]
        [TestCase(AlgoMode.TDES_CFB64, 64)]
        public void ShouldGenerateProperlySizedPayloadForEachGenerateCall(AlgoMode algo, int shift)
        {
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMMTEncrypt(group, new Random800_90(), _engineFactory.Object, _modeFactory.Object);
            for (int caseIdx = 0; caseIdx < _subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = _subject.Generate(group, false);
                Assume.That(result != null);
                Assume.That(result.Success);
                var testCase = result.TestCase;
                Assert.AreEqual((caseIdx + 1) * shift, testCase.PlainText.BitLength);
            }

        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldReturnAnErrorIfFails(AlgoMode algo)
        {
            _mode
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new SymmetricCipherResult("It broke"));
            var group = new TestGroup { Function = "encrypt", KeyingOption = 1, AlgoMode = algo };
            _subject = new TestCaseGeneratorMMTEncrypt(group, _random.Object, _engineFactory.Object, _modeFactory.Object);
            var result = _subject.Generate(group, false);
            Assert.IsFalse(result.Success);
        }
    }
}
