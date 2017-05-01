using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
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
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','), result.ErrorMessage);
        }

        [Test]
        [TestCase("Max invalid, negative", new int[] { 0, -1 })]
        [TestCase("min invalid, negative", new int[] { -1, -32 })]
        [TestCase("min gt max", new int[] { 20, 10 })]
        public void ShouldReturnErrorWithInvalidPtLength(string testLabel, int[] ptLengths)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPtLen(
                        new MathDomain()
                            .AddSegment(new ValueDomainSegment(ptLengths[0]))
                            .AddSegment(new ValueDomainSegment(ptLengths[1]))
                        )
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(new int[] { 128, -1, -2 }, 2)]
        [TestCase(new int[] { 128, -1, -2, -3 }, 3)]
        public void ShouldReturnErrorWithInvalidNonceLength(int[] nonceLengths, int errorsExpected)
        {
            MathDomain md = new MathDomain();
            nonceLengths.ToList().ForEach(fe => md.AddSegment(new ValueDomainSegment(fe)));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithNonceLen(md)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','), result.ErrorMessage);
        }

        [Test]
        [TestCase("Max invalid, negative", new int[] { 0, -1 })]
        [TestCase("min invalid, negative", new int[] { -1, -32 })]
        [TestCase("min gt max", new int[] { 20, 10 })]
        public void ShouldReturnErrorWithInvalidAadLength(string testLabel, int[] aadLengths)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAadLen(
                        new MathDomain()
                            .AddSegment(new ValueDomainSegment(aadLengths[0]))
                            .AddSegment(new ValueDomainSegment(aadLengths[1]))
                        )
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
        public void ShouldReturnErrorWithInvalidTagLength(int[] keyLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLen(keyLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','), result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusPtLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.PtLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusAadLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.AadLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusNonceLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.IvLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusTagLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.TagLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }
    }
}
