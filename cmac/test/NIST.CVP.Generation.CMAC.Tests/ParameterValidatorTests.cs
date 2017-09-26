using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.CMAC.AES;

namespace NIST.CVP.Generation.CMAC_AES.Tests
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
        [TestCase("CMAC-AES-128", true)]
        [TestCase("CMAC-AES-123", false)]
        [TestCase("badValue", false)]
        [TestCase(null, false)]
        public void ShouldReportFailureWithBadAlgorithm(string value, bool success)
        {
            Parameters p = new ParameterBuilder()
                .WithAlgorithm(value)
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
            Parameters p = new ParameterBuilder()
                .WithDirection(new [] { value })
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
            Parameters p = new ParameterBuilder()
                .WithMsgLen(value)
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
                            ParameterValidator.VALID_MAC_LENGTH_MAX,
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
            Parameters p = new ParameterBuilder()
                .WithMacLen(value)
                .Build();

            var result = _subject.Validate(p);

            Assert.AreEqual(success, result.Success);
        }
    }
}
