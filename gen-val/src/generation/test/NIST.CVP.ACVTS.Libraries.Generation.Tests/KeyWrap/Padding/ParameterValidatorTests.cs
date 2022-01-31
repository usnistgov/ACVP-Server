using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;
using NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.AES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using ParameterValidator = NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AESP.ParameterValidator;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.Padding
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
        [TestCase(KeyWrapType.AES_KWP)]
        public void ShouldValidateParametersWithDefaultBuilder(KeyWrapType keyWrapType)
        {
            Parameters p = new ParameterBuilder(keyWrapType).Build();

            var result = _subject.Validate(p);

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidKeyWrapType()
        {
            int invalidKeyWrapType = -1;
            KeyWrapType keyWrapType = (KeyWrapType)invalidKeyWrapType;

            Parameters p = new ParameterBuilder(keyWrapType).Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("none provided", new string[] { })]
        [TestCase("some invalid", new string[] { "encrypt", "notValid" })]
        [TestCase("some null", new string[] { "encrypt", null })]
        [TestCase("all invalid", new string[] { "notValid", null })]
        public void ShouldReturnErrorWithInvalidDirection(string testLabel, string[] directions)
        {
            Parameters p = new ParameterBuilder(KeyWrapType.AES_KWP)
                .WithDirection(directions)
                .Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("none provided", new string[] { })]
        [TestCase("some invalid", new string[] { "cipher", "notValid" })]
        [TestCase("some null", new string[] { "cipher", null })]
        [TestCase("all invalid", new string[] { "notValid", null })]
        public void ShouldReturnErrorWithInvalidKwCipher(string testLabel, string[] kwCiphers)
        {
            Parameters p = new ParameterBuilder(KeyWrapType.AES_KWP)
                .WithKwCipher(kwCiphers)
                .Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("none provided", new int[] { })]
        [TestCase("some invalid", new int[] { 128, 64 })]
        [TestCase("all invalid", new int[] { 64, 42 })]
        public void ShouldReturnErrorWithInvalidKeySize(string testLabel, int[] keySizes)
        {
            Parameters p = new ParameterBuilder(KeyWrapType.AES_KWP)
                .WithKeyLen(keySizes)
                .Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        #region GetInvalidPtLens
        static List<object[]> GetInvalidPtLens()
        {
            List<object[]> list = new List<object[]>()
            {
                new object[]
                {
                    "No segments",
                    new MathDomain()
                },
                new object[]
                {
                    "Below minimum",
                    new MathDomain()
                    .AddSegment(
                        new RangeDomainSegment(
                            new Random800_90(),
                            0,
                            ParameterValidator.MAXIMUM_PAYLOAD_LEN,
                            ParameterValidator.PT_MODULUS
                        )
                    )
                },
                new object[]
                {
                    "Bad modulus on value",
                    new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                96,
                                96+63,
                                63
                            )
                        )
                },
                new object[]
                {
                    "Bad modulus on increment",
                    new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                ParameterValidator.MINIMUM_PAYLOAD_LEN,
                                ParameterValidator.MAXIMUM_PAYLOAD_LEN,
                                4
                            )
                        )
                }
            };

            return list;
        }
        #endregion GetInvalidPtLens

        [Test]
        [TestCaseSource(nameof(GetInvalidPtLens))]
        public void ShouldReturnErrorWithPtLenInvalid(string label, MathDomain ptLen)
        {
            Parameters p = new ParameterBuilder(KeyWrapType.AES_KWP)
                .WithPtLens(ptLen)
                .Build();

            var result = _subject.Validate(p);

            Assert.IsFalse(result.Success);
        }
    }
}
