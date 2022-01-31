using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private ParameterValidator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new ParameterValidator();
        }

        [Test]
        public void ShouldReportSuccessWithValidParameters()
        {
            Parameters p = new ParameterBuilder().Build();

            var result = _subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("CMAC", "", false)]
        [TestCase("CMAC", null, false)]
        [TestCase("CMAC-AES", "", true)]
        [TestCase("CMAC-TDES", "", true)]
        [TestCase("badValue", "null", false)]
        [TestCase("badValue", null, false)]
        [TestCase(null, null, false)]
        public void ShouldReportFailureWithBadAlgorithm(string algo, string mode, bool success)
        {
            var c = new CapabilityBuilder()
                .WithKeyLen(algo?.ToLower() == "cmac-aes" ? 128 : 0)
                .WithKeyingOption(algo?.ToLower() == "cmac-tdes" ? 1 : 0)
                .Build();

            Parameters p = new ParameterBuilder()
                .WithAlgorithm(algo)
                .WithMode(mode)
                .WithCapabilities(new[] { c })
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }

        [Test]
        [TestCase("gen", true)]
        [TestCase("ver", true)]
        [TestCase("direction", false)]
        [TestCase("encrypt", false)]
        [TestCase(null, false)]
        public void ShouldReportFailureWithBadDirection(string value, bool success)
        {
            var c = new CapabilityBuilder()
                .WithDirection(value)
                .Build();

            Parameters p = new ParameterBuilder()
                .WithCapabilities(new[] { c })
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }

        private static object[] msgLenDomains = new[]
        {
            new object[]
            {
                "Success mod 8",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                true
            },
            new object[]
            {
                "Success 0",
                new MathDomain().AddSegment(new ValueDomainSegment(0)),
                true
            },
            new object[]
            {
                "Success full range mod 8",
                new MathDomain()
                    .AddSegment(
                        new RangeDomainSegment(
                            new Random800_90(),
                            ParameterValidator.VALID_MESSAGE_LENGTH_MIN,
                            ParameterValidator.VALID_MESSAGE_LENGTH_MAX,
                            8
                        )
                    ),
                true
            },
            new object[]
            {
                "Success full range mod 16",
                new MathDomain()
                    .AddSegment(
                        new RangeDomainSegment(
                            new Random800_90(),
                            ParameterValidator.VALID_MESSAGE_LENGTH_MIN,
                            ParameterValidator.VALID_MESSAGE_LENGTH_MAX,
                            8
                        )
                    ),
                true
            },
            new object[]
            {
                "Failure not mod 8",
                new MathDomain().AddSegment(new ValueDomainSegment(7)),
                false
            }
        };

        [Test]
        [TestCaseSource(nameof(msgLenDomains))]
        public void ShouldReportFailureWithBadMsgLenDomain(string label, MathDomain value, bool success)
        {
            var c = new CapabilityBuilder()
                .WithMsgLen(value)
                .Build();

            Parameters p = new ParameterBuilder()
                .WithCapabilities(new[] { c })
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }

        private static object[] macLenDomains = new[]
                {
            new object[]
            {
                "Success mod 8",
                new MathDomain().AddSegment(new ValueDomainSegment(64)),
                true
            },
            new object[]
            {
                "Failure 0",
                new MathDomain().AddSegment(new ValueDomainSegment(0)),
                false
            },
            new object[]
            {
                "Success full range mod 8",
                new MathDomain()
                    .AddSegment(
                        new RangeDomainSegment(
                            new Random800_90(),
                            ParameterValidator.VALID_MAC_LENGTH_MIN,
                            128,
                            1
                        )
                    ),
                true
            },
            new object[]
            {
                "Failure below minimum",
                new MathDomain().AddSegment(new ValueDomainSegment(0)),
                false
            },
            new object[]
            {
                "Failure above maximum",
                new MathDomain().AddSegment(new ValueDomainSegment(129)),
                false
            },
        };

        [Test]
        [TestCaseSource(nameof(macLenDomains))]
        public void ShouldReportFailureWithBadMacLenDomain(string label, MathDomain value, bool success)
        {
            var c = new CapabilityBuilder()
                .WithMacLen(value)
                .Build();

            Parameters p = new ParameterBuilder()
                .WithCapabilities(new[] { c })
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }

        [Test]
        [TestCase("CMAC-AES", 128, 0, "gen", true)]
        [TestCase("CMAC-AES", 192, 0, "gen", true)]
        [TestCase("CMAC-AES", 256, 0, "gen", true)]
        [TestCase("CMAC-TDES", 0, 1, "gen", true)]
        [TestCase("CMAC-TDES", 0, 2, "gen", false)] // gen not valid for keying option 2

        [TestCase("CMAC-AES", 128, 0, "ver", true)]
        [TestCase("CMAC-AES", 192, 0, "ver", true)]
        [TestCase("CMAC-AES", 256, 0, "ver", true)]
        [TestCase("CMAC-TDES", 0, 1, "ver", true)]
        [TestCase("CMAC-TDES", 0, 2, "ver", true)]

        // keying options not valid for AES
        [TestCase("CMAC-AES", 0, 1, "gen", false)]
        [TestCase("CMAC-AES", 0, 2, "gen", false)]
        [TestCase("CMAC-AES", 0, 1, "ver", false)]
        [TestCase("CMAC-AES", 0, 2, "ver", false)]

        // key length is not a thing for tdes
        [TestCase("CMAC-TDES", 128, 0, "gen", false)]
        [TestCase("CMAC-TDES", 128, 0, "ver", false)]
        [TestCase("CMAC-TDES", 192, 0, "gen", false)]
        [TestCase("CMAC-TDES", 192, 0, "ver", false)]
        [TestCase("CMAC-TDES", 256, 0, "gen", false)]
        [TestCase("CMAC-TDES", 256, 0, "ver", false)]

        // keying option 0 is not a thing for tdes
        [TestCase("CMAC-TDES", 0, 0, "gen", false)]
        [TestCase("CMAC-TDES", 0, 0, "ver", false)]

        // misc invalid
        [TestCase("invalid", 0, 0, "gen", false)]
        [TestCase("invalid", 0, 0, "ver", false)]
        [TestCase("invalid", 0, 1, "gen", false)]
        [TestCase("invalid", 0, 1, "ver", false)]
        [TestCase(null, 1, 0, "gen", false)]
        [TestCase(null, 1, 0, "ver", false)]
        [TestCase(null, 1, 1, "gen", false)]
        [TestCase(null, 1, 1, "ver", false)]
        [TestCase(null, 1, 0, null, false)]
        public void ShouldParseAesAndTdesProperly(string algo, int keyLen, int keyingOption, string direction, bool success)
        {
            var c = new CapabilityBuilder()
                .WithDirection(direction)
                .WithKeyLen(keyLen)
                .WithKeyingOption(keyingOption)
                .WithMacLen(new MathDomain().AddSegment(new ValueDomainSegment(algo?.ToLower() == "cmac-tdes" ? 64 : 128)))
                .Build();

            Parameters p = new ParameterBuilder()
                .WithAlgorithm(algo)
                .WithCapabilities(new[] { c })
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }
    }
}
