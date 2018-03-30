using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public void ShouldFailIfValueDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.SrtpKe = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.SrtpKe)));
        }

        [Test]
        public void ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.SrtcpKa = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.SrtcpKa)));
        }

        [Test]
        public void ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.SrtcpKs = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.SrtcpKs)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                SrtpKe = new BitString("01ABCDEF0123456789ABCDEF012345678901"),
                SrtpKa = new BitString("02ABCDEF0123456789ABCDEF012345678901"),
                SrtpKs = new BitString("03ABCDEF0123456789ABCDEF012345678901"),

                SrtcpKe = new BitString("04ABCDEF0123456789ABCDEF012345678901"),
                SrtcpKa = new BitString("05ABCDEF0123456789ABCDEF012345678901"),
                SrtcpKs = new BitString("06ABCDEF0123456789ABCDEF012345678901"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
