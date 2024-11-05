using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.PBKDF;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.PBKDF
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
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "SHA-1", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "SHA2-256", null } }
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

            Assert.That(result.Success, Is.False);
        }
    }
}
