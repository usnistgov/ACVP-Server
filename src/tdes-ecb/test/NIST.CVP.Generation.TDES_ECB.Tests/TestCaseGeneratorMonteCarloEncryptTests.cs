using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMonteCarloEncryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>> _mockMCT;
        private TestCaseGeneratorMonteCarloEncrypt _subject;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_ECB", true);
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(new BitString(1));
            _mockMCT = new Mock<IMonteCarloTester<Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse>>();
            _subject = new TestCaseGeneratorMonteCarloEncrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldCallRandomTwiceOnceForKeyOnceForCipherText(int keyingOption)
        {
            var testGroup = new TestGroup()
            {
                KeyingOption = keyingOption
            };

            _subject.Generate(testGroup, false);
            var numberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);
            _mockRandom.Verify(v => v.GetRandomBitString(64 * numberOfKeys), nameof(keyingOption));
            _mockRandom.Verify(v => v.GetRandomBitString(64));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            var testGroup = new TestGroup()
            {
                KeyingOption = 1
            };

            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromTestCaseMethod()
        {
            var testGroup = new TestGroup()
            {
                KeyingOption = 1
            };

            var testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            var errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new Crypto.Common.Symmetric.MCTResult<AlgoArrayResponse>(errorMessage));

            var testGroup = new TestGroup()
            {
                KeyingOption = 1
            };

            var testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            var errorMessage = "something bad happened! oh noes!";
            _mockMCT.Setup(s => s.ProcessMonteCarloTest(It.IsAny<IModeBlockCipherParameters>()))
                .Throws(new Exception(errorMessage));

            var testGroup = new TestGroup()
            {
                KeyingOption = 1
            };

            var testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
