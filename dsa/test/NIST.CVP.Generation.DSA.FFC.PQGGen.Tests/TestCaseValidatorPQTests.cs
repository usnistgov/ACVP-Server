using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorPQTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodPQ()
        {
            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<Counter>()))
                .Returns(new PQValidateResult());

            var subject = new TestCaseValidatorPQ(GetTestCase(), pqMock.Object);
            var result = subject.Validate(GetResultTestCase());

            pqMock.Verify(v => v.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<Counter>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadG()
        {
            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<Counter>()))
                .Returns(new PQValidateResult("Fail"));

            var subject = new TestCaseValidatorPQ(GetTestCase(), pqMock.Object);
            var result = subject.Validate(GetResultTestCase());

            pqMock.Verify(v => v.Validate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<Counter>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
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
                P = 2,
                Q = 3
            };
        }

        private Mock<IPQGeneratorValidator> GetPQMock()
        {
            return new Mock<IPQGeneratorValidator>();
        }
    }
}
