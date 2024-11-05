using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CCM
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.That(result.Success, Is.True);
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

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage.Count(c => c == ','), Is.EqualTo(errorsExpected), result.ErrorMessage);
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

            Assert.That(result.Success, Is.False);
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

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage.Count(c => c == ','), Is.EqualTo(errorsExpected), result.ErrorMessage);
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

            Assert.That(result.Success, Is.False);
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

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage.Count(c => c == ','), Is.EqualTo(errorsExpected), result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusPtLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusAadLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.AadLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidModulusNonceLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.IvLen = new MathDomain().AddSegment(new ValueDomainSegment(7));

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidTagLen()
        {
            Parameters p = new ParameterBuilder().Build();
            p.TagLen = new[] { 7 };

            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }
    }
}
