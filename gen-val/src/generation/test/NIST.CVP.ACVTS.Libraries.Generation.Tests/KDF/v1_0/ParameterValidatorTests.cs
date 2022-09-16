using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.v1_0
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(KdfModes.None)]
        public void ShouldReturnErrorWithInvalidKdfMode(KdfModes mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        new CapabilityBuilder().WithKdfMode(mode).Build()
                    })
                    .Build()
                );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        static object[] macTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new CounterLocations[] { } },
            new object[] { "Invalid value", new [] { MacModes.None } },
            new object[] { "Partially valid", new [] { MacModes.None, MacModes.HMAC_SHA3_224 } },
        };
        [Test]
        [TestCaseSource(nameof(macTestCases))]
        public void ShouldReturnErrorWithInvalidMacMode(string testCaseLabel, MacModes[] mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[] { new CapabilityBuilder().WithMacMode(mode).Build() })
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] counterLengthTestCases =
        {
            new object[] { "empty", new int[] { } },
            new object[] { "Invalid value", new [] { 1 } },
            new object[] { "Partially valid", new [] { 8, 1 } },
        };
        [Test]
        [TestCaseSource(nameof(counterLengthTestCases))]
        public void ShouldReturnErrorWithInvalidCounterLength(string testCaseLabel, int[] mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[] { new CapabilityBuilder().WithCounterLength(mode).Build() })
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] orderTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new CounterLocations[] { } },
            new object[] { "Partially valid", new [] { CounterLocations.BeforeFixedData, CounterLocations.None } },
        };
        [Test]
        [TestCaseSource(nameof(orderTestCases))]
        public void ShouldReturnErrorWithInvalidFixedDataOrder(string testCaseLabel, CounterLocations[] mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[] { new CapabilityBuilder().WithFixedDataOrder(mode).Build() })
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        #region GetInvalidDataLens
        static List<object[]> GetInvalidDataLens()
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
                                ParameterValidator.MAX_DATA_LENGTH
                            )
                        )
                },
                new object[]
                {
                    "Above maximum",
                    new MathDomain()
                        .AddSegment(
                            new RangeDomainSegment(
                                new Random800_90(),
                                ParameterValidator.MIN_DATA_LENGTH,
                                90000
                            )
                        )
                }
            };

            return list;
        }
        #endregion GetInvalidDataLens
        [Test]
        [TestCaseSource(nameof(GetInvalidDataLens))]
        public void ShouldReturnErrorWithPtLenInvalid(string label, MathDomain dataLen)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[] { new CapabilityBuilder().WithSupportedLengths(dataLen).Build() })
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, 0, true)]
        [TestCase(MacModes.CMAC_AES192, 0, true)]
        [TestCase(MacModes.CMAC_AES256, 0, true)]
        [TestCase(MacModes.CMAC_TDES, 0, true)]
        [TestCase(MacModes.HMAC_SHA1, 0, true)]
        [TestCase(MacModes.HMAC_SHA224, 0, true)]
        [TestCase(MacModes.HMAC_SHA256, 0, true)]
        [TestCase(MacModes.HMAC_SHA384, 0, true)]
        [TestCase(MacModes.HMAC_SHA512, 0, true)]
        [TestCase(MacModes.HMAC_SHA_d512t224, 0, true)]
        [TestCase(MacModes.HMAC_SHA_d512t256, 0, true)]
        [TestCase(MacModes.HMAC_SHA3_224, 0, true)]
        [TestCase(MacModes.HMAC_SHA3_256, 0, true)]
        [TestCase(MacModes.HMAC_SHA3_384, 0, true)]
        [TestCase(MacModes.HMAC_SHA3_512, 0, true)]

        [TestCase(MacModes.CMAC_AES128, 128, true)]
        [TestCase(MacModes.CMAC_AES192, 192, true)]
        [TestCase(MacModes.CMAC_AES256, 256, true)]
        [TestCase(MacModes.CMAC_TDES, 192, true)]
        [TestCase(MacModes.HMAC_SHA1, 160, true)]
        [TestCase(MacModes.HMAC_SHA224, 224, true)]
        [TestCase(MacModes.HMAC_SHA256, 256, true)]
        [TestCase(MacModes.HMAC_SHA384, 384, true)]
        [TestCase(MacModes.HMAC_SHA512, 512, true)]
        [TestCase(MacModes.HMAC_SHA_d512t224, 224, true)]
        [TestCase(MacModes.HMAC_SHA_d512t256, 256, true)]
        [TestCase(MacModes.HMAC_SHA3_224, 224, true)]
        [TestCase(MacModes.HMAC_SHA3_256, 256, true)]
        [TestCase(MacModes.HMAC_SHA3_384, 384, true)]
        [TestCase(MacModes.HMAC_SHA3_512, 512, true)]

        [TestCase(MacModes.CMAC_AES128, 1024, false)]
        [TestCase(MacModes.CMAC_AES192, 1024, false)]
        [TestCase(MacModes.CMAC_AES256, 1024, false)]
        [TestCase(MacModes.CMAC_TDES, 1024, false)]
        [TestCase(MacModes.HMAC_SHA1, 1024, true)]
        [TestCase(MacModes.HMAC_SHA224, 1024, true)]
        [TestCase(MacModes.HMAC_SHA256, 1024, true)]
        [TestCase(MacModes.HMAC_SHA384, 1024, true)]
        [TestCase(MacModes.HMAC_SHA512, 1024, true)]
        [TestCase(MacModes.HMAC_SHA_d512t224, 1024, true)]
        [TestCase(MacModes.HMAC_SHA_d512t256, 1024, true)]
        [TestCase(MacModes.HMAC_SHA3_224, 1024, true)]
        [TestCase(MacModes.HMAC_SHA3_256, 1024, true)]
        [TestCase(MacModes.HMAC_SHA3_384, 1024, true)]
        [TestCase(MacModes.HMAC_SHA3_512, 1024, true)]

        [TestCase(MacModes.CMAC_AES128, 1023, false)]
        [TestCase(MacModes.CMAC_AES192, 1023, false)]
        [TestCase(MacModes.CMAC_AES256, 1023, false)]
        [TestCase(MacModes.CMAC_TDES, 1023, false)]
        [TestCase(MacModes.HMAC_SHA1, 1023, false)]
        [TestCase(MacModes.HMAC_SHA224, 1023, false)]
        [TestCase(MacModes.HMAC_SHA256, 1023, false)]
        [TestCase(MacModes.HMAC_SHA384, 1023, false)]
        [TestCase(MacModes.HMAC_SHA512, 1023, false)]
        [TestCase(MacModes.HMAC_SHA_d512t224, 1023, false)]
        [TestCase(MacModes.HMAC_SHA_d512t256, 1023, false)]
        [TestCase(MacModes.HMAC_SHA3_224, 1023, false)]
        [TestCase(MacModes.HMAC_SHA3_256, 1023, false)]
        [TestCase(MacModes.HMAC_SHA3_384, 1023, false)]
        [TestCase(MacModes.HMAC_SHA3_512, 1023, false)]

        [TestCase(MacModes.CMAC_AES128, 8000, false)]
        [TestCase(MacModes.CMAC_AES192, 8000, false)]
        [TestCase(MacModes.CMAC_AES256, 8000, false)]
        [TestCase(MacModes.CMAC_TDES, 8000, false)]
        [TestCase(MacModes.HMAC_SHA1, 8000, false)]
        [TestCase(MacModes.HMAC_SHA224, 8000, false)]
        [TestCase(MacModes.HMAC_SHA256, 8000, false)]
        [TestCase(MacModes.HMAC_SHA384, 8000, false)]
        [TestCase(MacModes.HMAC_SHA512, 8000, false)]
        [TestCase(MacModes.HMAC_SHA_d512t224, 8000, false)]
        [TestCase(MacModes.HMAC_SHA_d512t256, 8000, false)]
        [TestCase(MacModes.HMAC_SHA3_224, 8000, false)]
        [TestCase(MacModes.HMAC_SHA3_256, 8000, false)]
        [TestCase(MacModes.HMAC_SHA3_384, 8000, false)]
        [TestCase(MacModes.HMAC_SHA3_512, 8000, false)]
        public void ShouldValidateCustomKeyInLenAsExpected(MacModes macMode, int customKeyInLen, bool isValid)
        {
            var subject = new ParameterValidator();
            var capability = new CapabilityBuilder()
                .WithMacMode(new[] { macMode })
                .WithCustomKeyInLength(customKeyInLen)
                .Build();

            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[] { capability })
                    .Build());

            Assert.AreEqual(isValid, result.Success);
        }
    }
}
