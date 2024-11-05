using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
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

        static object[] hashAlgTestCases =
        {
            new object[] {"null", null},
            new object[] {"empty", new string[] { }},
            new object[] {"Invalid value", new string[] {"notValid"}},
            new object[] {"Partially valid", new string[] {"sha-1", "notValid"}},
            new object[] {"Partially valid w/ null", new string[] {"sha2-256", null}}
        };

        [Test]
        [TestCaseSource(nameof(hashAlgTestCases))]
        public void ShouldReturnErrorWithInvalidHashAlg(string testCaseLabel, string[] tweakMode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(tweakMode)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidHashWithV12()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithVersion(new[] { TlsModes.v12 })
                    .WithHashAlg(new[] { "sha2-224", "sha2-256" })
                    .Build()
            );

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(512, 520, 1)]
        [TestCase(512, 2048, 8)]
        public void ShouldReturnErrorWithInvalidKeyBlockLengths(int min, int max, int step)
        {
            var domain = new MathDomain().AddSegment(new RangeDomainSegment(null, min, max, step));
            var subject = new ParameterValidator();
            var result = subject.Validate((new ParameterBuilder().WithAlgorithm("TLS-v1.2").WithMode("KDF").WithRevision("RFC7627").WithKeyBlockLength(domain).Build()));

            Assert.That(result.Success, Is.False);
        }
    }
}
