using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA1.Tests
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

        [Test]
        public void ShouldReturnErrorWithNullDigestLengthSupplied()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestLen(null)
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithNullMessageLengthSupplied()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMessageLen(null)
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(null, 1)]
        [TestCase(new int[] {}, 1)]
        [TestCase(new int[] {-1}, 1)]
        [TestCase(new int[] {160, -1}, 1)]
        [TestCase(new int[] {160, -1, -2}, 2)]
        [TestCase(new int[] {160, -1, -2, -3}, 3)]
        public void ShouldReturnErrorWithInvalidDigestLength(int[] digLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestLen(digLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ',') + 1);  // +1 reflects actual number of errors
        }

        [Test]
        [TestCase(null, 1)]
        [TestCase(new int[] {}, 1)]
        [TestCase(new int[] {-1}, 1)]
        [TestCase(new int[] {128, -1}, 1)]
        [TestCase(new int[] {128, -1, -2}, 2)]
        [TestCase(new int[] {128, -1, -2, -3}, 3)]
        public void ShouldReturnErrorWithInvalidMessageLength(int[] msgLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMessageLen(msgLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ',') + 1);
        }
    }
}
