using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>> _mockMct;
        private Mock<IMonteCarloFactoryTdes> _mockMctFactory;
        private TestCaseGeneratorMonteCarloDecrypt _subject;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_CBC", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(() => new BitString(64));
            _mockMct = new Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _mockMctFactory = new Mock<IMonteCarloFactoryTdes>();
            _mockMctFactory
                .Setup(s => s.GetInstance(
                    It.IsAny<BlockCipherModesOfOperation>())
                )
                .Returns(_mockMct.Object);
            _subject = new TestCaseGeneratorMonteCarloDecrypt(_mockRandom.Object, _mockMctFactory.Object);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldCallRandomTwiceOnceForKeyOnce(int keyingOption)
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = keyingOption
            };
            _subject.Generate(testGroup, false);

            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);
            _mockRandom.Verify(v => v.GetRandomBitString(64 * numberOfKeys), nameof(numberOfKeys));
            _mockRandom.Verify(v => v.GetRandomBitString(64));
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
            var testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            TestCase testCase = new TestCase()
            {
                Iv = new BitString(64),
                Key = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            _subject.Generate(testGroup, testCase);

            _mockMct.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMct.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyingOption = 1
            };
            TestCase testCase = new TestCase()
            {
                Iv = new BitString(64),
                Key = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
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
                Iv = new BitString(64),
                Key = new BitString(192),
                PlainText = new BitString(64),
                CipherText = new BitString(64)
            };
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
