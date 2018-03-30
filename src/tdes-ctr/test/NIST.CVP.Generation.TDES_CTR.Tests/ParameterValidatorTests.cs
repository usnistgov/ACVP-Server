using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(null, 0)]
        [TestCase(new int[] { }, 0)]
        [TestCase(new int[] { -1 }, 0)]
        [TestCase(new int[] { 1, -1 }, 0)]
        [TestCase(new int[] { 2, -1, -2 }, 1)]
        [TestCase(new int[] { 1, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidKeyingOption(int[] keyLengths, int errorsExpected)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyingOption(keyLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        static object[] directionTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new [] { "notValid" } },
            new object[] { "Partially valid", new [] { "encrypt", "notValid" } },
            new object[] { "Partially valid w/ null", new [] { "encrypt", null } }
        };
        [Test]
        [TestCaseSource(nameof(directionTestCases))]
        public void ShouldReturnErrorWithInvalidDirection(string testCaseLabel, string[] direction)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDirection(direction)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
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
                                ParameterValidator.MAXIMUM_DATA_LEN
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
                                60,
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
            var p = new ParameterBuilder()
                .WithDataLen(ptLen)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }
    }
}
