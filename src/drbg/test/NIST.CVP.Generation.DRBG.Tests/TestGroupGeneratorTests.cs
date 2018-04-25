using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Math.Domain;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private readonly ValueDomainSegment _valueDomainSegment = new ValueDomainSegment(42);
        private readonly RangeDomainSegment _rangeDomainSegment = new RangeDomainSegment(new Random800_90(), 0, 42, 2);
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

            Assert.Throws(typeof(ArgumentException), () => _subject.BuildTestGroups(p));
        }

        [Test]
        public void ShouldCreateOneGroupPerLengthParamterWhenMinAndMaxAreSame()
        {
            var p = GetParametersDomainsAsValue();

            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(1, result.Count);
        }

        private Parameters GetParametersDomainsAsValue()
        {
            MathDomain md = new MathDomain();
            md.AddSegment(_valueDomainSegment);

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-128",
                PredResistanceEnabled = new [] {true},
                Capabilities = new []
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

        private Parameters GetParametersDomainsAsRange()
        {
            MathDomain md = new MathDomain();
            md.AddSegment(_rangeDomainSegment);

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-128",
                PredResistanceEnabled = new [] {true},
                Capabilities = new []
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
