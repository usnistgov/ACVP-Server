using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.Shared
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private readonly ParameterValidator _subject = new();

        [Test]
        public void WhenGivenDefaultParameterBuilder_ShouldPass()
        {
            var pb = new ParameterBuilder();
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase("XMSS", "sigGen", "SP800-208", true)]
        [TestCase("XMSS", "sigVer", "SP800-208", true)]
        [TestCase("XMSS", "KeyGen", "SP800-208", false)]
        [TestCase(null, null, null, false)]
        public void WhenGivenAlgoModeRevision_ShouldVerifyOnlyValidCombinations(string algo, string mode, string revision, bool expectedSuccess)
        {
            var pb = new ParameterBuilder().WithAlgoModeRevision(algo, mode, revision);
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
        }

        [Test]
        public void WhenGivenNullCapabilities_ShouldFail()
        {
            var pb = new ParameterBuilder().WithGeneralCapabilities(null);
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        public void WhenGivenCapabilitiesWithNullModes_ShouldFail()
        {
            var pb = new ParameterBuilder()
                .WithGeneralCapabilities(new GeneralCapabilities { XmssModes = null });
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        public void WhenGivenCapabilitiesWithEmptyModes_ShouldFail()
        {
            var pb = new ParameterBuilder()
                .WithGeneralCapabilities(new GeneralCapabilities { XmssModes = [] });
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        public void WhenGivenGeneralCapabilities_AllValid_ShouldPass()
        {
            var capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_256, XmssMode.XMSS_SHAKE256_20_192]
            };

            var pb = new ParameterBuilder().WithGeneralCapabilities(capabilities);
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        public void WhenGivenGeneralCapabilities_InvalidXmssType_ShouldFail()
        {
            var capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.Invalid]
            };

            var pb = new ParameterBuilder().WithGeneralCapabilities(capabilities);
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        public void WhenGivenGeneralCapabilities_SomeInvalid_ShouldFail()
        {
            var capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_256, XmssMode.Invalid]
            };

            var pb = new ParameterBuilder().WithGeneralCapabilities(capabilities);
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        [TestCase(8, true)]
        [TestCase(65536, true)]
        [TestCase(-1, false)]
        [TestCase(4, false)]
        [TestCase(65600, false)]
        public void WhenGivenMessageLength_ShouldEnforceBounds(int messageLength, bool expectedSuccess)
        {
            var pb = new ParameterBuilder()
                .WithMessageLength(new MathDomain().AddSegment(new ValueDomainSegment(messageLength)));
            var p = pb.Build();
            var result = _subject.Validate(p);
            Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
        }
    }
}
