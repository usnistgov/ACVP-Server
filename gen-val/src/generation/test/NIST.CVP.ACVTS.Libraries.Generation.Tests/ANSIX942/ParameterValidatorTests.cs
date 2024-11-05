using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX942
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

        static object[] hashAlgTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "sha2-512", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "sha2-256", null } }
        };
        [Test]
        [TestCaseSource(nameof(hashAlgTestCases))]
        public void ShouldReturnErrorWithInvalidHashAlg(string testCaseLabel, string[] hashAlg)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(hashAlg)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        #region GetInvalidOtherInfoLens
        static List<object[]> GetInvalidOtherInfoLens()
        {
            var list = new List<object[]>
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
                                ParameterValidator.MIN_OTHER_INFO_LEN - 1,
                                ParameterValidator.MAX_OTHER_INFO_LEN
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
                                ParameterValidator.MIN_OTHER_INFO_LEN,
                                ParameterValidator.MAX_OTHER_INFO_LEN + 1
                            )
                        )
                }
            };
            return list;
        }
        #endregion GetInvalidOtherInfoLens
        [Test]
        [TestCaseSource(nameof(GetInvalidOtherInfoLens))]
        public void ShouldReturnErrorWithInvalidOtherInfoLen(string label, MathDomain len)
        {
            Parameters p = new ParameterBuilder()
                .WithOtherInfoLength(len)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }

        #region GetInvalidSuppInfoLens
        static List<object[]> GetInvalidSuppInfoLens()
        {
            var list = new List<object[]>
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
                                ParameterValidator.MIN_OTHER_INFO_LEN - 8,
                                ParameterValidator.MAX_OTHER_INFO_LEN
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
                                ParameterValidator.MIN_OTHER_INFO_LEN,
                                ParameterValidator.MAX_OTHER_INFO_LEN + 8
                            )
                        )
                },
                new object[]
                {
                    "Not mod8",
                    new MathDomain().AddSegment(new ValueDomainSegment(9))
                }
            };
            return list;
        }
        #endregion GetInvalidOtherInfoLens
        [Test]
        [TestCaseSource(nameof(GetInvalidSuppInfoLens))]
        public void ShouldReturnErrorWithInvalidSuppInfoLen(string label, MathDomain len)
        {
            Parameters p = new ParameterBuilder()
                .WithSuppInfoLength(len)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }

        #region GetInvalidKeyLens
        static List<object[]> GetInvalidKeyLens()
        {
            var list = new List<object[]>
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
                                ParameterValidator.MIN_KEY_LEN - 1,
                                ParameterValidator.MAX_KEY_LEN
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
                                ParameterValidator.MIN_KEY_LEN,
                                ParameterValidator.MAX_KEY_LEN + 1
                            )
                        )
                }
            };
            return list;
        }
        #endregion GetInvalidKeyLens
        [Test]
        [TestCaseSource(nameof(GetInvalidKeyLens))]
        public void ShouldReturnErrorWithInvalidKeyLen(string label, MathDomain len)
        {
            Parameters p = new ParameterBuilder()
                .WithKeyLength(len)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }
    }
}
