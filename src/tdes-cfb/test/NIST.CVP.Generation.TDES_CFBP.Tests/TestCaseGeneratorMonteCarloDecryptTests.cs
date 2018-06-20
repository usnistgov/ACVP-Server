using System;
using Moq;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs>>
            _mockMct;
        private Mock<IMonteCarloFactoryTdesPartitions> _mockMctFactory;
        private TestCaseGeneratorMonteCarloDecrypt _subject;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(() => new BitString(64));
            _mockMct =
                new Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs>>();
            _mockMctFactory = new Mock<IMonteCarloFactoryTdesPartitions>();
            _mockMctFactory
                .Setup(s => s.GetInstance(
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mockMct.Object);
        }

        [Test]
        [TestCase(1, AlgoMode.TDES_CFBP1)]
        [TestCase(2, AlgoMode.TDES_CFBP1)]
        [TestCase(1, AlgoMode.TDES_CFBP8)]
        [TestCase(2, AlgoMode.TDES_CFBP8)]
        [TestCase(1, AlgoMode.TDES_CFBP64)]
        [TestCase(2, AlgoMode.TDES_CFBP64)]
        public void ShouldCallAlgoFromIsSampleMethod(int keyingOption, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = keyingOption,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            _subject.Generate(testGroup, false);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        [TestCase(1, AlgoMode.TDES_CFBP1)]
        [TestCase(2, AlgoMode.TDES_CFBP1)]
        [TestCase(3, AlgoMode.TDES_CFBP1)]
        [TestCase(1, AlgoMode.TDES_CFBP8)]
        [TestCase(2, AlgoMode.TDES_CFBP8)]
        [TestCase(3, AlgoMode.TDES_CFBP8)]
        [TestCase(1, AlgoMode.TDES_CFBP64)]
        [TestCase(2, AlgoMode.TDES_CFBP64)]
        [TestCase(3, AlgoMode.TDES_CFBP64)]
        public void ShouldCallAlgoFromTestCaseMethod(int keyingOption, AlgoMode algoMode)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = keyingOption,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            _subject.Generate(testGroup, testCase);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        [TestCase(1, AlgoMode.TDES_CFBP1)]
        [TestCase(2, AlgoMode.TDES_CFBP1)]
        [TestCase(3, AlgoMode.TDES_CFBP1)]
        [TestCase(1, AlgoMode.TDES_CFBP8)]
        [TestCase(2, AlgoMode.TDES_CFBP8)]
        [TestCase(3, AlgoMode.TDES_CFBP8)]
        [TestCase(1, AlgoMode.TDES_CFBP64)]
        [TestCase(2, AlgoMode.TDES_CFBP64)]
        [TestCase(3, AlgoMode.TDES_CFBP64)]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful(int keyingOption, AlgoMode algoMode)
        {
            string errorMessage = "something bad happened!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new Crypto.Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = keyingOption,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
                Keys = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        [TestCase(1, AlgoMode.TDES_CFBP1)]
        [TestCase(2, AlgoMode.TDES_CFBP1)]
        [TestCase(3, AlgoMode.TDES_CFBP1)]
        [TestCase(1, AlgoMode.TDES_CFBP8)]
        [TestCase(2, AlgoMode.TDES_CFBP8)]
        [TestCase(3, AlgoMode.TDES_CFBP8)]
        [TestCase(1, AlgoMode.TDES_CFBP64)]
        [TestCase(2, AlgoMode.TDES_CFBP64)]
        [TestCase(3, AlgoMode.TDES_CFBP64)]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException(int keyingOption, AlgoMode algoMode)
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = keyingOption,
                AlgoMode = algoMode
            };
            _subject = new TestCaseGeneratorMonteCarloDecrypt(testGroup, _mockRandom.Object, _mockMctFactory.Object);
            TestCase testCase = new TestCase()
            {
                IV1 = new BitString(64),
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
