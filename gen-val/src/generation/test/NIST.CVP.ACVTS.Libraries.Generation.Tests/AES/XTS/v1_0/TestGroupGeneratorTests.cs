using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v1_0
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        #region GetParametersAndExpectedGroups
        private static List<object> GetParametersAndExpectedGroups()
        {
            var randy = new Random800_90();

            var list = new List<object>
            {
                new object[]
                {
                    "Minimal Inputs",
                    new string[] { "encrypt" },
                    new int[] { 128 },
                    new string[] { "hex" },
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    1
                },
                new object[]
                {
                    "Additional key w/ Minimal Inputs",
                    new string[] { "encrypt" },
                    new int[] { 128, 256 },
                    new string[] { "hex" },
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    2
                },
                new object[]
                {
                    "All inputs at maximum, except PtLen",
                    new string[] { "encrypt", "decrypt" },
                    new int[] { 128, 256 },
                    new string[] { "hex", "number" },
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    8
                },
                new object[]
                {
                    "Testing math domain, only 4 possible values",
                    new string[] { "encrypt" },
                    new int[] { 128 },
                    new string[] { "hex" },
                    new MathDomain().AddSegment(new RangeDomainSegment(randy, 128, 256)),
                    4
                },
                new object[]
                {
                    "Testing math domain, large range of values",
                    new string[] { "encrypt" },
                    new int[] { 256 },
                    new string[] { "hex" },
                    new MathDomain().AddSegment(new RangeDomainSegment(randy, 128, 128*200)),
                    5
                },
                new object[]
                {
                    "Maximum number of groups for a test vector set",
                    new string[] { "encrypt", "decrypt" },
                    new int[] { 128, 256 },
                    new string[] { "hex", "number" },
                    new MathDomain().AddSegment(new RangeDomainSegment(randy, 128, 128*200)),
                    40
                }
            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public async Task ShouldReturnOneITestGroupForEachCombination(string label, string[] direction, int[] keyLen, string[] tweakModes, MathDomain ptLen, int expectedResultCount)
        {
            var p = new Parameters
            {
                Algorithm = "AES",
                Mode = "XTS",
                KeyLen = keyLen,
                Direction = direction,
                TweakMode = tweakModes,
                PayloadLen = ptLen
            };

            _subject = new TestGroupGenerator();
            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
