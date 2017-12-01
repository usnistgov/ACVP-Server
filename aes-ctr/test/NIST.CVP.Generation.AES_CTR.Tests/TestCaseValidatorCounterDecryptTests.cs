using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorCounterDecryptTests
    {
        [Test]
        public void ShouldFailIfNoPlainTextIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PlainText = null;

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{suppliedTestCase.PlainText} was not present"));
        }

        [Test]
        public void ShouldFailIfNoIVsArePresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.IVs = null;

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedTestCase.IVs)} was not present"));
        }

        [Test]
        public void ShouldFailWithIncorrectNumberOfIVs()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.IVs = new List<BitString>();

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedTestCase.IVs)} does not have the correct number"), "Reason");
        }

        [Test]
        public void ShouldFailIfDeferredResolverFails()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult("fail"));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(GetTestCase());

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Server unable to complete"));
        }

        [Test]
        public void ShouldFailIfPlainTextDoesNotMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("BEEFFACE");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(GetTestCase().PlainText, GetTestCase().IVs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text does not match"));
        }

        [Test]
        public void ShouldRunDeferredResolverIfAllComponentsAreInPlace()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(GetTestCase().PlainText, GetTestCase().IVs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(GetTestCase());

            Assert.IsNotNull(result);

            deferredMock
                .Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public void ShouldReportSuccessOnValidTestCase()
        {
            var goodTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(goodTestCase.PlainText, goodTestCase.IVs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = subject.Validate(goodTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(128),
                Key = new BitString(128),
                Length = 128,
                IVs = new List<BitString>
                {
                    new BitString(128)
                },
                CipherText = new BitString(128)
            };
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult>> GetDeferredResolver()
        {
            return new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult>>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
