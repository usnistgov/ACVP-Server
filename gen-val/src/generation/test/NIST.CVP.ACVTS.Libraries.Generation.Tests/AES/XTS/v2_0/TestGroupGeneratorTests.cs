using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        #region GetParametersAndExpectedGroups
        private static List<object> GetParametersAndExpectedGroups()
        {
            var list = new List<object>
            {
                new object[]
                {
                    "Minimal Inputs",
                    new [] { BlockCipherDirections.Encrypt },
                    new [] { 128 },
                    new [] { XtsTweakModes.Hex },
                    true,
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    1
                },
                new object[]
                {
                    "Additional key w/ Minimal Inputs",
                    new [] { BlockCipherDirections.Encrypt },
                    new [] { 128, 256 },
                    new [] { XtsTweakModes.Hex },
                    true,
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    2
                },
                new object[]
                {
                    "All inputs at maximum with matching PtLen",
                    new [] { BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt },
                    new [] { 128, 256 },
                    new [] { XtsTweakModes.Hex, XtsTweakModes.Number },
                    true,
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    8
                },
                new object[]
                {
                    "All inputs at maximum without matching PtLen",
                    new [] { BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt },
                    new [] { 128, 256 },
                    new [] { XtsTweakModes.Hex, XtsTweakModes.Number },
                    false,
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    new MathDomain().AddSegment(new ValueDomainSegment(128)),
                    16
                }
            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public async Task ShouldReturnOneITestGroupForEachCombination(string label, BlockCipherDirections[] direction, int[] keyLen, XtsTweakModes[] tweakModes, bool ptLenMatch, MathDomain ptLen, MathDomain dataUnitLen, int expectedResultCount)
        {
            var p = new Parameters
            {
                Algorithm = "AES",
                Mode = "XTS",
                Revision = "2.0",
                KeyLen = keyLen,
                Direction = direction,
                TweakMode = tweakModes,
                PayloadLen = ptLen,
                DataUnitLenMatchesPayload = ptLenMatch,
                DataUnitLen = dataUnitLen
            };

            _subject = new TestGroupGenerator();
            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
