using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        static object[] keyTestCases =
        {
            new object[] {"empty", new int[] { }},
            new object[] {"Invalid value", new [] {-1}},
            new object[] {"Partially valid", new [] {128, 257}},
        };

        [Test]
        [TestCaseSource(nameof(keyTestCases))]
        public void ShouldReturnErrorWithInvalidKeyLength(string testCaseLabel, int[] keyLength)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLength(keyLength)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] kdrTestCases =
        {
            new object[] {"empty", new int[] { }},
            new object[] {"Invalid value", new [] {-1}},
            new object[] {"Partially valid", new [] {5, 25}},
        };

        [Test]
        [TestCaseSource(nameof(kdrTestCases))]
        public void ShouldReturnErrorWithInvalidKdr(string testCaseLabel, int[] kdr)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKdr(kdr)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }
    }
}
