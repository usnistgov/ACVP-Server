using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.TDES
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGenerator();
        }

        #region GetParametersAndExpectedGroups
        private static List<object> GetParametersAndExpectedGroups()
        {
            Random800_90 randy = new Random800_90();

            List<object> list = new List<object>()
            {
                new object[]
                {
                    "Minimal Inputs",
                    "TDES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain().AddSegment(new ValueDomainSegment(128)), // 1
                    2 // ! * 1 * 1 * 1 * 1
                },

                new object[]
                {
                    "Testing math domain, large range of values, 2 pulled",
                    "TDES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 2 values total from range
                    4 // 1 * 1 * 1 * 1 * 5
                },
                new object[]
                {
                    "Maximum number of groups for a test vector set",
                    "TDES-KW", // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt", "decrypt" }, // 2
                    new string[] { "cipher", "inverse" }, // 2
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 2 values total from range
                    16 // 1 * 1 * 2 * 2 * 2
                }

            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public void ShouldCreateCorrectNumberOfGroups(
            string testLabel,
            string algorithm,
            int[] keyLen,
            string[] direction,
            string[] kwCipher,
            MathDomain ptLen,
            int expectedNumberOfGroups)
        {
            Parameters parameters = new Parameters()
            {
                Algorithm = algorithm,
                KeyingOption = new[] { 1, 2 },
                KwCipher = kwCipher,
                Direction = direction,
                PtLen = ptLen,
                //KeyLen = keyLen,
            };

            var result = _subject.BuildTestGroups(parameters);

            Assert.AreEqual(expectedNumberOfGroups, result.Count());
        }
    }
}
