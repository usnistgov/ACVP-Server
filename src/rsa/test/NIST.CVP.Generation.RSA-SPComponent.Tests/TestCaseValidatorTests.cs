using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithCorrectSignature()
        {
            var subject = new TestCaseValidator(GetTestCase());
            var result = subject.Validate(GetTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithIncorrectSignature()
        {
            var responseTestCase = GetTestCase();
            responseTestCase.Signature = new BitString("1234");

            var subject = new TestCaseValidator(GetTestCase());
            var result = subject.Validate(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithMismatchedFailingTests()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = subject.Validate(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailIfNoSignatureProvidedWhenExpected()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetTestCase());
            var result = subject.Validate(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithMatchingFailureTests()
        {
            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = subject.Validate(GetFailureTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Signature = new BitString("ABCD"),
                TestPassed = true
            };
        }

        private TestCase GetFailureTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = false
            };
        }
    }
}
