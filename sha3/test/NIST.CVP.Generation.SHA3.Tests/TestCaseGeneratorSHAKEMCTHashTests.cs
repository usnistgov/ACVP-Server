using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSHAKEMCTHashTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<ISHA3_MCT> _mockMCT;
        private TestCaseGeneratorSHAKEMCTHash _subject;

        [SetUp]
        public void SetUp()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<ISHA3_MCT>();
            _subject = new TestCaseGeneratorSHAKEMCTHash(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallAlgoHashFromIsSampleGenerateMethod()
        {
            var testGroup = GetTestGroup();
            _subject.Generate(testGroup, false);
            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void ShouldCallAlgoHashFromTestCaseGenerateMethod()
        {
            var testGroup = GetTestGroup();
            var testCase = new TestCase();

            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            var errorMessage = "something bad happened!";
            _mockMCT.Setup(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
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
            _mockMCT.Setup(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
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
                Function = "shake",
                DigestSize = 128,
                MinOutputLength = 16,
                MaxOutputLength = 65536
            };
        }
    }
}
