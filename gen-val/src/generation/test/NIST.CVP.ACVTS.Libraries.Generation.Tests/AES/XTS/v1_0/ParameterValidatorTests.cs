using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v1_0
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { -1 }, 0)]
        [TestCase(new int[] { 128, -1 }, 0)]
        [TestCase(new int[] { 128, -1, -2 }, 1)]
        [TestCase(new int[] { 128, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidKeyLength(int[] keyLengths, int errorsExpected)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLen(keyLengths)
                    .Build()
            );

            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage.Count(c => c == ','), Is.EqualTo(errorsExpected));
        }

        static object[] directionTestCases = new object[]
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "encrypt", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "encrypt", null } }
        };
        [Test]
        [TestCaseSource(nameof(directionTestCases))]
        public void ShouldReturnErrorWithInvalidDirection(string testCaseLabel, string[] direction)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDirection(direction)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
        }

        static object[] tweakTestCases = new object[]
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new string[] { "notValid" } },
            new object[] { "Partially valid", new string[] { "hex", "notValid" } },
            new object[] { "Partially valid w/ null", new string[] { "number", null } }
        };
        [Test]
        [TestCaseSource(nameof(tweakTestCases))]
        public void ShouldReturnErrorWithInvalidTweakMode(string testCaseLabel, string[] tweakMode)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithTweakMode(tweakMode)
                    .Build()
            );

            Assert.That(result.Success, Is.False, testCaseLabel);
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
                                ParameterValidator.MAXIMUM_PT_LEN
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
                                128,
                                90000
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
            Parameters p = new ParameterBuilder()
                .WithPtLen(ptLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.That(result.Success, Is.False);
        }
    }
}
