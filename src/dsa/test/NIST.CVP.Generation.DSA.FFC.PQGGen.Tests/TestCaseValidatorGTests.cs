using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Generation.Core;
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
            var subject = new TestCaseValidatorG(GetTestCase(), GetTestGroup(), GetResolverMock(true).Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadG()
        {
            var subject = new TestCaseValidatorG(GetTestCase(), GetTestGroup(), GetResolverMock(false).Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                TestGroupId = 1
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

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, GValidateResult>> GetResolverMock(bool shouldPass)
        {
            var goodResult = new GValidateResult();
            var badResult = new GValidateResult("fail");

            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, GValidateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }

        private Mock<IGGeneratorValidator> GetGMock()
        {
            return new Mock<IGGeneratorValidator>();
        }
    }
}
