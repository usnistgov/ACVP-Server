using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success);
        }

        static object[] directionTestCases = new object[]
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "encrypt", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "encrypt", null } }
        };
        [Test]
        [TestCaseSource(nameof(directionTestCases))]
        public void ShouldReturnErrorWithInvalidDirection(string testCaseLabel, string[] direction)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMode(direction)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] failKeyingOptionTestCases = new object[]
        {
            new object[] { "Zero", new int[] { 0 }},
            new object[] { "Three", new int[] { 3 }},
            new object[] { "Four", new int[] { 4 }},
            new object[] { "Partially valid", new int[] { 1, 4 }}
        };
        [Test]
        [TestCaseSource(nameof(failKeyingOptionTestCases))]
        public void ShouldReturnErrorWithInvalidKeyingOption(string testCaseLabel, int[] keyingOption)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyingOptions(keyingOption)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] successKeyingOptionTestCases = new object[]
        {
            new object[] { "OneTwo", new int[] { 1, 2}},
            new object[] { "One", new int[] { 1 }},
            new object[] { "Two", new int[] { 2 }}
        };
        [Test]
        [TestCaseSource(nameof(successKeyingOptionTestCases))]
        public void ShouldReturnNoErrorWithValidKeyingOption(string testCaseLabel, int[] keyingOption)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyingOptions(keyingOption)
                    .Build()
            );

            Assert.IsTrue(result.Success, testCaseLabel);
        }

        private class ParameterBuilder
        {
            private string _algorithm;
            private string[] _mode;
            private int[] _keyingOptions;

            public ParameterBuilder()
            {
                // Provides a valid (as of construction) set of parameters
                _algorithm = "TDES_CFBP";
                _mode = ParameterValidator.VALID_DIRECTIONS;
                _keyingOptions = ParameterValidator.VALID_KEYING_OPTIONS;
            }

            public ParameterBuilder WithMode(string[] value)
            {
                _mode = value;
                return this;
            }

            public ParameterBuilder WithKeyingOptions(int[] value)
            {
                _keyingOptions = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters()
                {
                    Algorithm = _algorithm,
                    Direction = _mode,
                    KeyingOption = _keyingOptions
                };
            }
        }
    }
}
