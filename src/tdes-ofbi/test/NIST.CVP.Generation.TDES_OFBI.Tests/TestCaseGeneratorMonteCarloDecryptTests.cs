using Moq;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;
using System;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_OFBI.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloDecryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<ITDES_OFBI_MCT> _mockMCT;
        private TestCaseGeneratorMonteCarloDecrypt _subject;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Utilities.ConfigureLogging("TDES_OFB");
        }

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockRandom.Setup(s => s.GetRandomBitString(It.IsAny<int>())).Returns(new BitString(1));
            _mockMCT = new Mock<ITDES_OFBI_MCT>();
            _subject = new TestCaseGeneratorMonteCarloDecrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            var testGroup = new TestGroup
            {
                KeyingOption = 1
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromTestCaseMethod()
        {
            var testGroup = new TestGroup
            {
                KeyingOption = 1
            };

            var testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            var errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new MCTResult<AlgoArrayResponseWithIvs>(errorMessage));

            var testGroup = new TestGroup
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
            _mockMCT.Setup(s => s.MCTDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception(errorMessage));

            var testGroup = new TestGroup
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
