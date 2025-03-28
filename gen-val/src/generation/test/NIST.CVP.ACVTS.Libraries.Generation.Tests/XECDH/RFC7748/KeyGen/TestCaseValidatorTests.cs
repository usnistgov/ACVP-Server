using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.KeyGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWhenDeferredCryptoFails()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new XecdhKeyPairGenerateResult("fail")));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        private XecdhKeyPair GetKey(bool correctKey)
        {
            return correctKey ? new XecdhKeyPair(new Math.BitString("E0AF7652CB9608B4D736339264A0ECE38F6D55664F2DB10D3C12A5BA24DF785F"), new Math.BitString("D33C613C91CD7DBB4443614B9EF55828E0E43031DF34625E1AEFF2046A2380DE")) : new XecdhKeyPair(new Math.BitString("FACE"), new Math.BitString("BEEF"));
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult>> GetDeferredResolver(bool shouldPass)
        {
            var goodResult = Task.FromResult(new XecdhKeyPairGenerateResult(GetKey(true)));
            var badResult = Task.FromResult(new XecdhKeyPairGenerateResult(GetKey(false)));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
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
                KeyPair = GetKey(true)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.Curve25519,
            };
        }
    }
}
