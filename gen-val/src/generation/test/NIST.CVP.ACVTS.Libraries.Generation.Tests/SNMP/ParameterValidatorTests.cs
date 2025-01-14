﻿using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SNMP
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

        static object[] engineIdTestCases =
        {
            new object[] {"null", null},
            new object[] {"empty", new string[] { }},
            new object[] {"Invalid value", new[] {"notValid"}},
            new object[] {"Partially valid", new[] {"1234567812345678", "abcd"}},
            new object[] {"Partially valid w/ null", new[] {"abcdefabcdefabcdefabcdef", null}}
        };

        [Test]
        [TestCaseSource(nameof(engineIdTestCases))]
        public void ShouldReturnErrorWithInvalidEngineId(string testCaseLabel, string[] engineId)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithEngineId(engineId)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        static object[] passwordTestCases =
        {
            new object[]
            {
                "below minimum",
                new MathDomain().AddSegment(new ValueDomainSegment(1))
            },
            new object[]
            {
                "above maximum",
                new MathDomain().AddSegment(new ValueDomainSegment(65536)),
            },
            new object[]
            {
                "valid minimum but above maximum",
                new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 10000, 8)),
            }
        };

        [Test]
        [TestCaseSource(nameof(passwordTestCases))]
        public void ShouldReturnErrorWithInvalidPasswordLength(string testCaseLabel, MathDomain passwordLengths)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPasswordLength(passwordLengths)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }
    }
}
