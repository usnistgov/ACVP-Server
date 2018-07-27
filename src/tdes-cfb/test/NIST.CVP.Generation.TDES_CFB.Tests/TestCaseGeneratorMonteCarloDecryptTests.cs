using Moq;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;
using System;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CFB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>
            _mockMct;
        private Mock<IMonteCarloFactoryTdes> _mockMctFactory;
        private TestCaseGeneratorMonteCarloDecrypt _subject;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(() => new BitString(64));
            _mockMct =
                new Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _mockMctFactory = new Mock<IMonteCarloFactoryTdes>();
            _mockMctFactory
                .Setup(s => s.GetInstance(
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mockMct.Object);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldCallAlgoFromIsSampleMethod(AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            _subject.Generate(testGroup, false);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldCallAlgoFromTestCaseMethod(AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                Iv = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            _subject.Generate(testGroup, testCase);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful(AlgoMode algoMode)
        {
            string errorMessage = "something bad happened!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                Iv = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        [TestCase(AlgoMode.TDES_CFB1)]
        [TestCase(AlgoMode.TDES_CFB8)]
        [TestCase(AlgoMode.TDES_CFB64)]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException(AlgoMode algoMode)
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                Iv = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
