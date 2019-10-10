using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Generation.KDF.Tests
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
    }
}
