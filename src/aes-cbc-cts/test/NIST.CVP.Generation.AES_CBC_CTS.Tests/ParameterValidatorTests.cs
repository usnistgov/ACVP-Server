using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.AES_CBC_CTS.Tests
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
                    .WithMode(direction)
                    .Build()
            );

            Assert.IsFalse(result.Success, testCaseLabel);
        }

        static object[] messageLenTestCases = new object[]
        {
            new object[]
            {
                "test1 full domain",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 1)),
                true
            },
            new object[]
            {
                "test2 below min",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 127, 65536, 1)),
                false
            },
            new object[]
            {
                "test3 above max",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65537, 1)),
                false
            },
            new object[]
            {
                "test4 only block size",
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 128)),
                false
            },
            new object[]
            {
                "test5 multiple segments",
                new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536, 128))
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 192, 1920, 64)),
                true
            },
        };
        [Test]
        [TestCaseSource(nameof(messageLenTestCases))]
        public void ShouldFailInvalidMessageLens(string testCaseLabel, MathDomain domain, bool shouldPass)
        {
            ParameterValidator subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMessageLen(domain)
                    .Build()
            );

            Assert.AreEqual(result.Success, shouldPass, testCaseLabel);
        }

    }
}
