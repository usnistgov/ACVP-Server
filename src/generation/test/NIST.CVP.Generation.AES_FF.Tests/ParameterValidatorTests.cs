using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.IsTrue(result.Success);
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

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorsExpected, result.ErrorMessage.Count(c => c == ','));
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

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] tweakLenTestCases = new object[]
        {
            new object[]
            {
                "test1 full domain",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)),
                true
            },
            new object[]
            {
                "test2 below min",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), -8, 128, 8)),
                false
            },
            new object[]
            {
                "test3 above max",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 192, 8)),
                false
            },
            new object[]
            {
                "test4 invalid modulus",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 1)),
                false
            },
            new object[]
            {
                "test5 multiple segments",
                new MathDomain()
                    .AddSegment(new ValueDomainSegment(0))
                    .AddSegment(new ValueDomainSegment(128)),
                true
            },
        };
        [Test]
        [TestCaseSource(nameof(tweakLenTestCases))]
        public void ShouldFailInvalidTweakLens(string testCaseLabel, MathDomain domain, bool shouldPass)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("ACVP-AES-FF1")
                    .WithTweakLen(domain)
                    .Build()
            );

            Assert.AreEqual(result.Success, shouldPass, testCaseLabel);
        }

    }
}
