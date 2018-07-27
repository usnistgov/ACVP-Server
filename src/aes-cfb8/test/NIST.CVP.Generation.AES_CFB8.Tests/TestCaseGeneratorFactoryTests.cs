using System;
using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB8.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;
        private Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>
            _mockMct;
        private Mock<IMonteCarloFactoryAes> _mockMctFactory;
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IBlockCipherEngineFactory> _engineFactory;
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _mode;
        private Mock<IModeBlockCipherFactory> _modeFactory;

        [SetUp]
        public void Setup()
        {
            _engine = new Mock<IBlockCipherEngine>();
            _mockMct = new Mock<IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _mockMct
                .Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(() => new MCTResult<AlgoArrayResponse>(new List<AlgoArrayResponse>()));
            _mockMctFactory = new Mock<IMonteCarloFactoryAes>();
            _mockMctFactory
                .Setup(s => s.GetInstance(It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(_mockMct.Object);
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
            _subject = new TestCaseGeneratorFactory(null, _mockMctFactory.Object, _engineFactory.Object, _modeFactory.Object);
        }

        [Test]
        [TestCase("not relevant", 128, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 128, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 128, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 128, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 128, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 128, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 128, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 128, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 128, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 128, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 128, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 128, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 128, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 128, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 192, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 192, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 192, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 192, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 192, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 192, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 192, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 192, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 192, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 192, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 192, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 192, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 192, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 192, "", typeof(TestCaseGeneratorNull))]

        [TestCase("not relevant", 256, "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", 256, "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", 256, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Encrypt", 256, "MmT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("ENcrypt", 256, "MMT", typeof(TestCaseGeneratorMMTEncrypt))]
        [TestCase("Decrypt", 256, "mMT", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("decrypt", 256, "MMt", typeof(TestCaseGeneratorMMTDecrypt))]
        [TestCase("encrypt", 256, "mCt", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Encrypt", 256, "MCT", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("ENcrypt", 256, "mct", typeof(TestCaseGeneratorMCTEncrypt))]
        [TestCase("Decrypt", 256, "mct", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("decrypt", 256, "McT", typeof(TestCaseGeneratorMCTDecrypt))]
        [TestCase("Junk", 256, "", typeof(TestCaseGeneratorNull))]
        [TestCase("", 256, "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, int keySize, string testType, Type expectedType)
        {
            TestGroup testGroup = new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                Function = direction,
                KeyLength = keySize,
                TestType = testType
            };

            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var generator = _subject.GetCaseGenerator(new TestGroup()
            {
                AlgoMode = Common.AlgoMode.AES_CFB1,
                Function = string.Empty,
                TestType = string.Empty
            });
            Assert.IsNotNull(generator);
        }
    }
}
