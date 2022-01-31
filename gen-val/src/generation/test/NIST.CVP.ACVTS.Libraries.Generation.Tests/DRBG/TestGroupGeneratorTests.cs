using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DRBG
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private readonly ValueDomainSegment _valueDomainSegment = new ValueDomainSegment(42);
        private TestGroupGenerator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGenerator();
        }

        [Test]
        [TestCase("", "")]
        [TestCase("ctrDRBG", "invalid")]
        [TestCase("invalid", "AES-128")]
        [TestCase("hashDRBG", "AES-256")]
        [TestCase("ctrDRBG", "SHA-1")]
        [TestCase("hmacDRBG", "TDES")]
        public void ShouldThrowExceptionOnInvalidMechanismModeCombination(string mechanism, string mode)
        {
            var p = GetParametersDomainsAsValue();
            p.Algorithm = mechanism;
            p.Capabilities.First().Mode = mode;

            Assert.Throws(typeof(ArgumentException), () => _subject.BuildTestGroupsAsync(p));
        }

        [Test]
        public async Task ShouldCreateOneGroupPerLengthParamterWhenMinAndMaxAreSame()
        {
            var p = GetParametersDomainsAsValue();

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        [TestCase("ctrDRBG", "AES-128", false, DrbgMechanism.Counter, DrbgMode.AES128)]
        [TestCase("ctrDRBG", "AES-192", false, DrbgMechanism.Counter, DrbgMode.AES192)]
        [TestCase("ctrDRBG", "AES-256", false, DrbgMechanism.Counter, DrbgMode.AES256)]
        [TestCase("ctrDRBG", "TDES", false, DrbgMechanism.Counter, DrbgMode.TDES)]

        [TestCase("ctrDRBG", "AES-128", true, DrbgMechanism.Counter, DrbgMode.AES128)]
        [TestCase("ctrDRBG", "AES-192", true, DrbgMechanism.Counter, DrbgMode.AES192)]
        [TestCase("ctrDRBG", "AES-256", true, DrbgMechanism.Counter, DrbgMode.AES256)]
        [TestCase("ctrDRBG", "TDES", true, DrbgMechanism.Counter, DrbgMode.TDES)]

        [TestCase("hashDRBG", "SHA-1", false, DrbgMechanism.Hash, DrbgMode.SHA1)]
        [TestCase("hashDRBG", "SHA2-224", false, DrbgMechanism.Hash, DrbgMode.SHA224)]
        [TestCase("hashDRBG", "SHA2-256", false, DrbgMechanism.Hash, DrbgMode.SHA256)]
        [TestCase("hashDRBG", "SHA2-384", false, DrbgMechanism.Hash, DrbgMode.SHA384)]
        [TestCase("hashDRBG", "SHA2-512", false, DrbgMechanism.Hash, DrbgMode.SHA512)]
        [TestCase("hashDRBG", "SHA2-512/224", false, DrbgMechanism.Hash, DrbgMode.SHA512t224)]
        [TestCase("hashDRBG", "SHA2-512/256", false, DrbgMechanism.Hash, DrbgMode.SHA512t256)]
        [TestCase("hmacDRBG", "SHA-1", false, DrbgMechanism.HMAC, DrbgMode.SHA1)]
        [TestCase("hmacDRBG", "SHA2-224", false, DrbgMechanism.HMAC, DrbgMode.SHA224)]
        [TestCase("hmacDRBG", "SHA2-256", false, DrbgMechanism.HMAC, DrbgMode.SHA256)]
        [TestCase("hmacDRBG", "SHA2-384", false, DrbgMechanism.HMAC, DrbgMode.SHA384)]
        [TestCase("hmacDRBG", "SHA2-512", false, DrbgMechanism.HMAC, DrbgMode.SHA512)]
        [TestCase("hmacDRBG", "SHA2-512/224", false, DrbgMechanism.HMAC, DrbgMode.SHA512t224)]
        [TestCase("hmacDRBG", "SHA2-512/256", false, DrbgMechanism.HMAC, DrbgMode.SHA512t256)]

        public async Task ShouldCreateMechanismModeCorrectly(
            string algorithm,
            string mode,
            bool derFunc,
            DrbgMechanism expectedMechanism,
            DrbgMode expectedDrbgMode
        )
        {
            var p = new Parameters()
            {
                Algorithm = algorithm,
                PredResistanceEnabled = new[] { false, true },
                Capabilities = new List<Capability>()
                {
                    new Capability()
                    {
                        Mode = mode,
                        AdditionalInputLen = new MathDomain().AddSegment(_valueDomainSegment),
                        EntropyInputLen = new MathDomain().AddSegment(_valueDomainSegment),
                        NonceLen = new MathDomain().AddSegment(_valueDomainSegment),
                        PersoStringLen = new MathDomain().AddSegment(_valueDomainSegment),
                        DerFuncEnabled = derFunc
                    }
                }.ToArray()
            };

            var results = await _subject.BuildTestGroupsAsync(p);
            Assert.That(results.Count() == 2, "Should only be working with a 2 groups");

            foreach (TestGroup result in results)
            {
                Assert.AreEqual(
                    expectedMechanism,
                    result.DrbgParameters.Mechanism,
                    nameof(expectedMechanism)
                );
                Assert.AreEqual(
                    expectedDrbgMode,
                    result.DrbgParameters.Mode,
                    nameof(expectedDrbgMode)
                );
            }
        }

        private Parameters GetParametersDomainsAsValue()
        {
            MathDomain md = new MathDomain();
            md.AddSegment(_valueDomainSegment);

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-128",
                PredResistanceEnabled = new[] { true },
                Capabilities = new[]
                {
                    new Capability
                    {
                        Mode = "AES-128",
                        NonceLen = md,
                        AdditionalInputLen = md,
                        PersoStringLen = md,
                        EntropyInputLen = md
                    }
                }
            };

            return p;
        }
    }
}
