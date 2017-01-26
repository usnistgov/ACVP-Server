using Moq;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMCTHashTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<ISHA1_MCT> _mockMCT;
        private TestCaseGeneratorMCTHash _subject;

        [SetUp]
        public void SetUp()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<ISHA1_MCT>();
            _subject = new TestCaseGeneratorMCTHash(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallRandomOnceForMessage()
        {
            // No random call needed for digest... just message
            var messageLength = 160;
            TestGroup testGroup = new TestGroup()
            {
                MessageLength = messageLength
            };

            _subject.Generate(testGroup, false);

            _mockRandom.Verify(v => v.GetRandomBitString(messageLength), nameof(messageLength));
        }

        [Test]
        public void ShouldCallAlgoHashFromIsSampleMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                MessageLength = 160
            };

            _subject.Generate(testGroup, false);

            _mockMCT.Verify(v => v.MCTHash(It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldCallAlgoDecryptFromTestCaseMethod()
        {
            TestGroup testGroup = new TestGroup()
            {
                MessageLength = 160
            };

            TestCase testCase = new TestCase();
            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTHash(It.IsAny<BitString>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            string errorMessage = "Error here!";
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<BitString>()))
                .Returns(new MCTResult(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                MessageLength = 160
            };

            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            string errorMessage = "Error here!";
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<BitString>()))
                .Throws(new Exception(errorMessage));

            TestGroup testGroup = new TestGroup()
            {
                MessageLength = 160
            };

            TestCase testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
}
