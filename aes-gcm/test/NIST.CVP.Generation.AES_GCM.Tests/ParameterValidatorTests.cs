using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithNullTagSupplied()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithTagLen(null)
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithNullPtLengthSupplied()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPtLen(null)
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { -1 }, 0)]
        [TestCase(new int[] { 128, -1 }, 0)]
        [TestCase(new int[] { 128, -1, -2 }, 1)]
        [TestCase(new int[] { 128, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidKeyLength(int[] keyLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLen(keyLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
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

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { -1 }, 0)]
        [TestCase(new int[] { 128, -1 }, 0)]
        [TestCase(new int[] { 128, -1, -2 }, 1)]
        [TestCase(new int[] { 128, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidTagLength(int[] tagLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithTagLen(tagLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        [Test]
        // invalid range
        [TestCase(new int[] { -128 }, 0)]
        [TestCase(new int[] { 128, -128 }, 0)]
        [TestCase(new int[] { 128, -128, -256 }, 1)]
        [TestCase(new int[] { 128, -128, -256, -384 }, 2)] 
        // invalid multiple
        [TestCase(new int[] { 128, 1, 2 }, 1)]
        [TestCase(new int[] { 128, 1, 2, 3 }, 2)]
        public void ShouldReturnErrorWithInvalidPtLength(int[] ptLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPtLen(ptLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        [Test]
        // invalid range
        [TestCase(new int[] { -128 }, 0)]
        [TestCase(new int[] { 128, -128 }, 0)]
        [TestCase(new int[] { 128, -128, -256 }, 1)]
        [TestCase(new int[] { 128, -128, -256, -384 }, 2)]
        // invalid multiple
        [TestCase(new int[] { 128, 1, 2 }, 1)]
        [TestCase(new int[] { 128, 1, 2, 3 }, 2)]
        public void ShouldReturnErrorWithInvalidAadLength(int[] aadLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAadLen(aadLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        [Test]
        // invalid range
        [TestCase(new int[] { -128 }, 0)]
        [TestCase(new int[] { 0 }, 0)]
        [TestCase(new int[] { 128, -128 }, 0)]
        [TestCase(new int[] { 128, -128, -256 }, 1)]
        [TestCase(new int[] { 128, -128, -256, -384 }, 2)]
        // invalid multiple
        [TestCase(new int[] { 128, 9, 10 }, 1)]
        [TestCase(new int[] { 128, 9, 10, 11 }, 2)]
        public void ShouldReturnErrorWithInvalidIvLength(int[] ivLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithIvLen(ivLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        [Test]
        [TestCase("internal", true)]
        [TestCase("external", true)]
        [TestCase("invalid", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ShouldReturnErrorWithInvalidIvGen(string ivGen, bool isValid)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithIvGen(ivGen)
                    .Build()
            );

            Assert.AreEqual(isValid, result.Success);
        }

        [Test]
        [TestCase("8.2.1", true)]
        [TestCase("8.2.2", true)]
        [TestCase("invalid", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ShouldReturnErrorWithInvalidIvGenMode(string ivGenMode, bool isValid)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithIvGen("internal")
                    .WithIvGenMode(ivGenMode)
                    .Build()
            );

            Assert.AreEqual(isValid, result.Success);
        }

        private class ParameterBuilder
        {
            private string _algorithm;
            private string[] _mode;
            private int[] _keyLen;
            private int[] _ptLen;
            private int[] _ivLen;
            private string _ivGen;
            private string _ivGenMode;
            private int[] _aadLen;
            private int[] _tagLen;

            public ParameterBuilder()
            {
                // Provides a valid (as of construction) set of parameters
                _algorithm = "AES-GCM";
                _mode = ParameterValidator.VALID_DIRECTIONS;
                _keyLen = ParameterValidator.VALID_KEY_SIZES;
                _ptLen = new int[] { 128 };
                _ivLen = new int[] { 96 };
                _ivGen = ParameterValidator.VALID_IV_GEN[0];
                _ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0];
                _aadLen = new int[] { 128 };
                _tagLen = ParameterValidator.VALID_TAG_LENGTHS;
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

            public ParameterBuilder WithKeyLen(int[] value)
            {
                _keyLen = value;
                return this;
            }

            public ParameterBuilder WithPtLen(int[] value)
            {
                _ptLen = value;
                return this;
            }

            public ParameterBuilder WithIvLen(int[] value)
            {
                _ivLen = value;
                return this;
            }

            public ParameterBuilder WithIvGen(string value)
            {
                _ivGen = value;
                return this;
            }

            public ParameterBuilder WithIvGenMode(string value)
            {
                _ivGenMode = value;
                return this;
            }

            public ParameterBuilder WithAadLen(int[] value)
            {
                _aadLen = value;
                return this;
            }

            public ParameterBuilder WithTagLen(int[] value)
            {
                _tagLen = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters()
                {
                    aadLen = _aadLen,
                    Algorithm = _algorithm,
                    ivGen = _ivGen,
                    ivGenMode = _ivGenMode,
                    ivLen = _ivLen,
                    KeyLen = _keyLen,
                    Mode = _mode,
                    PtLen = _ptLen,
                    TagLen = _tagLen
                };
            }
        }
    }
}
