using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var parameterBuilder = new ParameterBuilder();
            var result = subject.Validate(parameterBuilder.Build());

            Assert.IsNull(result.ErrorMessage);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("null", new object[] { null })]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid valid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "SHA3", "notValid" })]
        public void ShouldReturnErrorWithInvalidMode(string label, object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMode(strModes)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid valid", new object[] { 0 })]
        [TestCase("Partially valid", new object[] { 224, 16 })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var digs = digestSize.Select(v => (int)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestSize(digs)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string[] _mode;
            private int[] _digestSize;

            public ParameterBuilder()
            {
                _algorithm = "SHA3";
                _mode = new [] {"SHA3", "SHAKE"};
                _digestSize = new [] { 256, 128 };
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithMode(string[] value)
            {
                _mode = value;
                return this;
            }

            public ParameterBuilder WithDigestSize(int[] value)
            {
                _digestSize = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    DigestSize = _digestSize,
                    Function = _mode
                };
            }
        }
    }
}
