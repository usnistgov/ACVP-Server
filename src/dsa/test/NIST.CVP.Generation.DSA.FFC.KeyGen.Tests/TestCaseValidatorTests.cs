using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var deferredMock = GetDeferredMock(true);

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), deferredMock.Object);
            var result = subject.Validate(GetResultTestCase());

            deferredMock.Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var deferredMock = GetDeferredMock(false);

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), deferredMock.Object);
            var result = subject.Validate(GetResultTestCase());

            deferredMock.Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);

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
                Key = new FfcKeyPair(1, 2)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
            };
        }
        
        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, FfcKeyPairValidateResult>> GetDeferredMock(bool shouldPass)
        {
            var goodResult = new FfcKeyPairValidateResult();
            var badResult = new FfcKeyPairValidateResult("fail");

            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, FfcKeyPairValidateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
