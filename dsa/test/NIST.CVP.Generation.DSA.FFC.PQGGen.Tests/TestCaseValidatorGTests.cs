using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodG()
        {
            var gMock = GetGMock();
            gMock
                .Setup(s => s.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()))
                .Returns(new GValidateResult());

            var subject = new TestCaseValidatorG(GetTestCase(), gMock.Object);
            var result = subject.Validate(GetResultTestCase());

            gMock.Verify(v => v.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()), Times.Once);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadG()
        {
            var gMock = GetGMock();
            gMock
                .Setup(s => s.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()))
                .Returns(new GValidateResult("Fail"));

            var subject = new TestCaseValidatorG(GetTestCase(), gMock.Object);
            var result = subject.Validate(GetResultTestCase());

            gMock.Verify(v => v.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()), Times.Once);
            Assert.AreEqual("failed", result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                G = 2
            };
        }

        private Mock<IGGeneratorValidator> GetGMock()
        {
            return new Mock<IGGeneratorValidator>();
        }
    }
}
