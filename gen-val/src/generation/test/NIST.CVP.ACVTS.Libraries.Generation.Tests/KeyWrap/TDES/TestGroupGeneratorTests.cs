using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.TDES
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
                    KeyWrapType.TDES_KW, // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain().AddSegment(new ValueDomainSegment(128)), // 1
                    1 // 1 * 1 * 1 * 1 * 1
                },

                new object[]
                {
                    "Testing math domain, large range of values, 2 pulled",
                    KeyWrapType.TDES_KW, // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt" }, // 1
                    new string[] { "cipher" }, // 1
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 2 values total from range
                    2 // 1 * 1 * 1 * 1 * 2
                },
                new object[]
                {
                    "Maximum number of groups for a test vector set",
                    KeyWrapType.TDES_KW, // 1
                    new int[] { 128 }, // 1
                    new string[] { "encrypt", "decrypt" }, // 2
                    new string[] { "cipher", "inverse" }, // 2
                    new MathDomain()
                        .AddSegment(new RangeDomainSegment(randy, 128, 128*200, 64)), // lots of possible values
                    // 2 values total from range
                    8 // 1 * 1 * 2 * 2 * 2
                }

            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public async Task ShouldCreateCorrectNumberOfGroups(
            string testLabel,
            KeyWrapType algorithm,
            int[] keyLen,
            string[] direction,
            string[] kwCipher,
            MathDomain ptLen,
            int expectedNumberOfGroups)
        {
            var parameters = new ParameterBuilder(algorithm)
                .WithKwCipher(kwCipher)
                .WithDirection(direction)
                .WithPtLens(ptLen)
                .Build();

            var result = await _subject.BuildTestGroupsAsync(parameters);

            Assert.AreEqual(expectedNumberOfGroups, result.Count());
        }
    }
}
