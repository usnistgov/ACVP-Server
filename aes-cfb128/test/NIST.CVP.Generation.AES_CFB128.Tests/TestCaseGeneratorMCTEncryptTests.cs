using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB128;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB128.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorMCTEncryptTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<IAES_CFB128_MCT> _mockMCT;
        private TestCaseGeneratorMCTEncrypt _subject;

        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<IAES_CFB128_MCT>();
            _subject = new TestCaseGeneratorMCTEncrypt(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallRandomTwiceOnceForKeyOnceForCipherText()
        {
            int keyLength = 256;
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = keyLength
            };
            _subject.Generate(testGroup, false);

            _mockRandom.Verify(v => v.GetRandomBitString(keyLength), nameof(keyLength));
            _mockRandom.Verify(v => v.GetRandomBitString(128));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldCallAlgoEncryptFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "something bad happened! oh noes!";
            _mockMCT.Setup(s => s.MCTEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                KeyLength = 128
            };
            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
