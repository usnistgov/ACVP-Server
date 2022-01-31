using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR
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
        [TestCase(new[] { -1 }, 0)]
        [TestCase(new[] { 128, -1 }, 0)]
        [TestCase(new[] { 128, -1, -2 }, 1)]
        [TestCase(new[] { 128, -1, -2, -3 }, 2)]
        public void ShouldReturnErrorWithInvalidKeyLength(int[] keyLengths, int errorsExpected)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyLen(keyLengths)
                    .Build()
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
        }

        static object[] _directionTestCases =
        {
            new object[] { "null", null },
            new object[] { "empty", new string[] { } },
            new object[] { "Invalid value", new [] { "notValid" } },
            new object[] { "Partially valid", new [] { "encrypt", "notValid" } },
            new object[] { "Partially valid w/ null", new [] { "encrypt", null } }
        };
        [Test]
        [TestCaseSource(nameof(_directionTestCases))]
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
                                120,
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

        [Test]
        public void ShouldPassRfc()
        {
            var p = new ParameterBuilder()
                .WithConformances(new[] { "RFC3686" })
                .WithIncrementalCounter(true)
                .WithIvGenMode(IvGenModes.External)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        public void ShouldFailWithBadConformance()
        {
            var p = new ParameterBuilder()
                .WithConformances(new[] { "rfc3686" })
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldFailWithNonIncrementalCounterRfc()
        {
            var p = new ParameterBuilder()
                .WithConformances(new[] { "RFC3686" })
                .WithIncrementalCounter(false)
                .WithIvGenMode(IvGenModes.External)
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldFailWithNoIvGenMode()
        {
            var p = new ParameterBuilder()
                .WithConformances(new[] { "RFC3686" })
                .Build();

            var subject = new ParameterValidator();
            var result = subject.Validate(p);

            Assert.IsFalse(result.Success);
        }
    }
}
