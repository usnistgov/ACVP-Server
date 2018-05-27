using Moq;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;
using System;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_OFBI.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloEncryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs>> _mockMct;
        private Mock<IMonteCarloFactoryTdesPartitions> _mockMctFactory;
        private TestCaseGeneratorMonteCarloEncrypt _subject;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_CFBI", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(() => new BitString(64));
            _mockMct = new Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs>>();
            _mockMctFactory = new Mock<IMonteCarloFactoryTdesPartitions>();
            _mockMctFactory
                .Setup(s => s.GetInstance(
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mockMct.Object);
            _subject = new TestCaseGeneratorMonteCarloEncrypt(_mockRandom.Object, _mockMctFactory.Object);
        }

        [Test]
        public void ShouldCallAlgoFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            _subject.Generate(testGroup, false);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldCallAlgoFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(192),
                CipherText = new BitString(102)
            };
            _subject.Generate(testGroup, testCase);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(192),
                CipherText = new BitString(102)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(192),
                CipherText = new BitString(102)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
