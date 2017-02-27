using System;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloHashTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<ISHA_MCT> _mockMCT;
        private TestCaseGeneratorMonteCarloHash _subject;

        [SetUp]
        public void SetUp()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<ISHA_MCT>();
            _subject = new TestCaseGeneratorMonteCarloHash(_mockRandom.Object, _mockMCT.Object, true);
        }

        [Test]
        public void ShouldCallAlgoHashFromIsSampleGenerateMethod()
        {
            var testGroup = GetTestGroup();
            _subject.Generate(testGroup, false);
            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>()));
        }

        [Test]
        public void ShouldCallAlgoHashFromTestCaseGenerateMethod()
        {
            var testGroup = GetTestGroup();
            var testCase = new TestCase();

            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            var errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new MCTResult<AlgoArrayResponse>(errorMessage));

            var testGroup = GetTestGroup();
            var testCase = new TestCase();

            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoFailsWithException()
        {
            var errorMessage = "something bad happened! oh noes!";
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Throws(new Exception(errorMessage));

            var testGroup = GetTestGroup();
            var testCase = new TestCase();
            var result = _subject.Generate(testGroup, testCase);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Function = ModeValues.SHA2,
                DigestSize = DigestSizes.d224
            };
        }
    }
}
