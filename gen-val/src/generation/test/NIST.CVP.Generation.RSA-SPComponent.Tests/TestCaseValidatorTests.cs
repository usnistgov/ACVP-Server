﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.RSA.v1_0.SpComponent;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithCorrectSignature()
        {
            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithIncorrectSignature()
        {
            var responseTestCase = GetTestCase();
            responseTestCase.Signature = new BitString("1234");

            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithMismatchedFailingTests()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailIfNoSignatureProvidedWhenExpected()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithMatchingFailureTests()
        {
            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = await subject.ValidateAsync(GetFailureTestCase());

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
