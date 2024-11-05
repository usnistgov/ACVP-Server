using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.KMAC
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

        [Test]
        [TestCase(new [] {MacModes.None})]
        [TestCase(new [] {MacModes.KMAC_128, MacModes.CMAC_AES128})]
        [TestCase(new [] {MacModes.None, MacModes.HMAC_SHA3_384})]
        [TestCase(new [] {MacModes.KMAC_128, MacModes.KMAC_256, MacModes.CMAC_TDES})]
        public void ShouldReturnErrorWithInvalidMacMode(MacModes[] mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().WithMacMode(mode).Build());
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        private static List<MathDomain> GetInvalidDomains(int min, int max)
        {
            return new List<MathDomain>
            {
                new (),
                new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                0,
                                max
                            )
                        ),
                new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                min,
                                90000
                            )
                        )
            };
        }

        [Test]
        public void ShouldReturnErrorWithInvalidKeyDerivationKeyLength()
        {
            var domains = GetInvalidDomains(ParameterValidator.MIN_KEY_DERIVATION_KEY_LENGTH, ParameterValidator.MAX_KEY_DERIVATION_KEY_LENGTH);
            foreach (var domain in domains)
            {
                var subject = new ParameterValidator();
                var result = subject.Validate(new ParameterBuilder().WithKeyDerivationKeyLength(domain).Build());
                Assert.That(result.Success, Is.False);
            }
        }
        
        [Test]
        public void ShouldReturnErrorWithInvalidDerivedKeyLength()
        {
            var domains = GetInvalidDomains(ParameterValidator.MIN_KEY_DERIVATION_KEY_LENGTH, ParameterValidator.MAX_KEY_DERIVATION_KEY_LENGTH);
            foreach (var domain in domains)
            {
                var subject = new ParameterValidator();
                var result = subject.Validate(new ParameterBuilder().WithDerivedKeyLength(domain).Build());
                Assert.That(result.Success, Is.False);
            }
        }
        
        [Test]
        public void ShouldReturnErrorWithInvalidContextLength()
        {
            var domains = GetInvalidDomains(ParameterValidator.MIN_OTHER_INFO_LENGTH, ParameterValidator.MAX_OTHER_INFO_LENGTH);
            foreach (var domain in domains)
            {
                var subject = new ParameterValidator();
                var result = subject.Validate(new ParameterBuilder().WithContextLength(domain).Build());
                Assert.That(result.Success, Is.False);
            }
        }
        
        [Test]
        public void ShouldReturnErrorWithInvalidLabelLength()
        {
            var domains = GetInvalidDomains(ParameterValidator.MIN_OTHER_INFO_LENGTH, ParameterValidator.MAX_OTHER_INFO_LENGTH);
            foreach (var domain in domains)
            {
                var subject = new ParameterValidator();
                var result = subject.Validate(new ParameterBuilder().WithLabelLength(domain).Build());
                Assert.That(result.Success, Is.False);
            }
        }

        [Test]
        public void ShouldReturnSuccessWithNullLabel()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().WithLabelLength(null).Build());
            Assert.That(result.Success, Is.True);
        }
    }
}
