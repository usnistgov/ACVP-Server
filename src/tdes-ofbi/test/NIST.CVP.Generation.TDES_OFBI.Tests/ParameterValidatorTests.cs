using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFBI.Tests
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

        static object[] directionTestCases = {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new[] { "notValid" } },
            new object[] { "Partially valid", new [] { "encrypt", "notValid" } },
            new object[] { "Partially valid w/ null", new [] { "encrypt", null } }
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

        static object[] failKeyingOptionTestCases = {
            new object[] { "Zero", new [] { 0 }},
            new object[] { "Three", new [] { 3 }},
            new object[] { "Four", new [] { 4 }},
            new object[] { "Partially valid", new [] { 1, 4 }}
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

        static object[] successKeyingOptionTestCases = {
            new object[] { "OneTwo", new [] { 1, 2}},
            new object[] { "One", new [] { 1 }},
            new object[] { "Two", new [] { 2 }}
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
                _algorithm = "TDES_OFBI";
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
