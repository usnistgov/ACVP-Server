using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Generation.Core;
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
            var subject = new TestCaseValidatorPQ(GetTestCase(), GetTestGroup(), GetResolverMock(true).Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadG()
        {
            var subject = new TestCaseValidatorPQ(GetTestCase(), GetTestGroup(), GetResolverMock(false).Object);
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
                P = 2,
                Q = 3
            };
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, PQValidateResult>> GetResolverMock(bool shouldPass)
        {
            var goodResult = new PQValidateResult();
            var badResult = new PQValidateResult("fail");

            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, PQValidateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
