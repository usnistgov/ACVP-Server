﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SRTP
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
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

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        static object[] kdrTestCases =
        {
            //new object[] {"empty", new int[] { }},
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

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        static IEnumerable<object> atLeastOneTests = new List<object>()
        {
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(new int[] { 1 })
                    .WithZeroKdr(true)
                    .Build(),
                true
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(new int[] { 1 })
                    .WithZeroKdr(false)
                    .Build(),
                true
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(new int[] {})
                    .WithZeroKdr(true)
                    .Build(),
                true
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(null)
                    .WithZeroKdr(true)
                    .Build(),
                true
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(null)
                    .WithZeroKdr(false)
                    .Build(),
                false
            },
            new object[]
            {
                new ParameterBuilder()
                    .WithKdr(new int[] {})
                    .WithZeroKdr(false)
                    .Build(),
                false
            },
        };

        [Test]
        [TestCaseSource(nameof(atLeastOneTests))]
        public void ShouldValidateKdrOptions(Parameters param, bool shouldValidate)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                param
            );

            Assert.That(result.Success, Is.EqualTo(shouldValidate));
        }
    }
}
