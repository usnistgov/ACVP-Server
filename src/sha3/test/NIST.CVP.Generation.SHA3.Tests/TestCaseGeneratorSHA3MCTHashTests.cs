using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSHA3MCTHashTests
    {
        private Mock<IRandom800_90> _mockRandom;
        private Mock<ISHA3_MCT> _mockMCT;
        private TestCaseGeneratorSHA3MCTHash _subject;

        [SetUp]
        public void SetUp()
        {
            _mockRandom = new Mock<IRandom800_90>();
            _mockMCT = new Mock<ISHA3_MCT>();
            _subject = new TestCaseGeneratorSHA3MCTHash(_mockRandom.Object, _mockMCT.Object);
        }

        [Test]
        public void ShouldCallAlgoHashFromIsSampleGenerateMethod()
        {
            var testGroup = GetTestGroup();
            _subject.Generate(testGroup, false);
            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<MathDomain>(), It.IsAny<bool>()));
        }

        [Test]
        public void ShouldCallAlgoHashFromTestCaseGenerateMethod()
        {
            var testGroup = GetTestGroup();
            var testCase = new TestCase();

            _subject.Generate(testGroup, testCase);

            _mockMCT.Verify(v => v.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<MathDomain>(), It.IsAny<bool>()));
        }

        [Test]
        public void ShouldReturnErrorMessageIfAlgoNotSuccessful()
        {
            var errorMessage = "something bad happened!";
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<MathDomain>(), It.IsAny<bool>()))
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
            _mockMCT.Setup(s => s.MCTHash(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<MathDomain>(), It.IsAny<bool>()))
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
                Function = "SHA3",
                DigestSize = 224
            };
        }
    }
}
