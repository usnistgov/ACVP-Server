using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.Tests
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

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        #region GetInvalidSharedInfoLens
        static List<object[]> GetInvalidSharedInfoLens()
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
                                ParameterValidator.SHARED_INFO_MINIMUM - 1,
                                ParameterValidator.SHARED_INFO_MAXIMUM
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
                                ParameterValidator.SHARED_INFO_MINIMUM,
                                ParameterValidator.SHARED_INFO_MAXIMUM + 1
                            )
                        )
                }
            };
            return list;
        }
        #endregion GetInvalidSharedInfoLens
        [Test]
        [TestCaseSource(nameof(GetInvalidSharedInfoLens))]
        public void ShouldReturnErrorWithInvalidSharedInfoLen(string label, MathDomain len)
        {
            Parameters p = new ParameterBuilder()
                .WithSharedInfoLength(len)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        #region GetInvalidKeyDataLens
        static List<object[]> GetInvalidKeyDataLens()
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
                                ParameterValidator.KEY_LENGTH_MINIMUM - 1,
                                ParameterValidator.KEY_LENGTH_MAXIMUM
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
                                ParameterValidator.KEY_LENGTH_MINIMUM,
                                ParameterValidator.KEY_LENGTH_MAXIMUM + 1
                            )
                        )
                }
            };
            return list;
        }
        #endregion GetInvalidKeyDataLens
        [Test]
        [TestCaseSource(nameof(GetInvalidKeyDataLens))]
        public void ShouldReturnErrorWithInvalidKeyDataLen(string label, MathDomain len)
        {
            Parameters p = new ParameterBuilder()
                .WithKeyDataLength(len)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        static object[] fieldSizeTestCases =
        {
            new object[] {"empty", new int[] { }},
            new object[] {"Invalid value", new [] {-1}},
            new object[] {"Partially valid", new [] {224, 512}},
        };
        [Test]
        [TestCaseSource(nameof(fieldSizeTestCases))]
        public void ShouldReturnErrorWithInvalidKdr(string testCaseLabel, int[] fieldSize)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFieldSize(fieldSize)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        [Test]
        public void ShouldReturnErrorWithInvalidCombinationOfFieldSizeAndHashAlg()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFieldSize(new[] {521})
                    .WithHashAlg(new[] {"sha2-224"})
                    .Build()
                );

            Assert.IsFalse(result.Success);
        }
    }
}
