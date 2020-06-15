using System.Collections.Generic;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.PBKDF.Tests
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
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "sha-1", "notValid" } },
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

        [Test]
        public void ShouldRejectSlowShaWithLargeIterationCount()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlg(new[] {"SHA3-224"})
                    .WithIterationCount(new MathDomain().AddSegment(new ValueDomainSegment(10000000)))
                    .Build()
            );
            
            Assert.IsFalse(result.Success);
        }

        #region GetInvalidKeyLengths
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
        #endregion GetInvalidNonceLens
        [Test]
        [TestCaseSource(nameof(GetInvalidKeyLens))]
        public void ShouldReturnErrorWithInvalidKeyLen(string label, MathDomain keyLen)
        {
            Parameters p = new ParameterBuilder()
                .WithKeyLength(keyLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }
    }
}