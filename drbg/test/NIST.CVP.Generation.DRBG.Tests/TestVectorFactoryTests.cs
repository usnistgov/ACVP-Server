using System;
using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {

        private readonly ValueDomainSegment _valueDomainSegment = new ValueDomainSegment(42);
        private readonly RangeDomainSegment _rangeDomainSegment = new RangeDomainSegment(new Random800_90(), 0, 42, 2);
        private TestVectorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestVectorFactory();
        }

        [Test]
        [TestCase("", "")]
        [TestCase("ctrDRBG", "invalid")]
        [TestCase("invalid", "AES-128")]
        public void ShouldThrowExceptionOnInvalidMechanismModeCombination(string mechanism, string mode)
        {
            var p = GetParametersDomainsAsValue();
            p.Algorithm = mechanism;
            p.Mode = mode;

            Assert.Throws(typeof(ArgumentException), () => _subject.BuildTestVectorSet(p));
        }

        [Test]
        public void ShouldCreateTwoGroupsForEachLengthParameterWhenMinAndMaxAreDifferent()
        {
            var p = GetParametersDomainsAsRange();

            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(16, result.TestGroups.Count);
        }

        [Test]
        public void ShouldCreateOneGroupPerLengthParamterWhenMinAndMaxAreSame()
        {
            var p = GetParametersDomainsAsValue();

            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(1, result.TestGroups.Count);
        }

        private Parameters GetParametersDomainsAsValue()
        {
            MathDomain md = new MathDomain();
            md.AddSegment(_valueDomainSegment);

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-128",
                NonceLen = md,
                AdditionalInputLen = md,
                PersoStringLen = md,
                EntropyInputLen = md
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
                NonceLen = md,
                AdditionalInputLen = md,
                PersoStringLen = md,
                EntropyInputLen = md
            };

            return p;
        }
    }
}
