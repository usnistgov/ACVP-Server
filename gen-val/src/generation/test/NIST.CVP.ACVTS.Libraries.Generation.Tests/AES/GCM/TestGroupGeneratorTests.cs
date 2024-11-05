using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.GCM
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

        [Test]
        [TestCase(
            "test4",
            new string[] { "encrypt", "decrypt" },
            new int[] { 128, 192, 256 },
            new int[] { 0, 128 },
            new int[] { 96 },
            "external",
            "",
            new int[] { 0, 128 },
            new int[] { 128 }
        )]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            string[] mode,
            int[] keyLen,
            int[] ptLen,
            int[] ivLen,
            string ivGen,
            string ivGenMode,
            int[] aadLen,
            int[] tagLen
        )
        {
            MathDomain mdPt = GetMathDomainFromArray(ptLen);
            MathDomain mdIv = GetMathDomainFromArray(ivLen);
            MathDomain mdAad = GetMathDomainFromArray(aadLen);

            Parameters p = new Parameters()
            {
                AadLen = mdAad,
                Algorithm = "ACVP-AES-GCM",
                Revision = "1.0",
                IvGen = ivGen,
                IvGenMode = ivGenMode,
                IvLen = mdIv,
                KeyLen = keyLen,
                Direction = mode,
                PayloadLen = mdPt,
                TagLen = tagLen
            };

            var lengths = new List<int> { ptLen.Length, ivLen.Length, aadLen.Length, tagLen.Length };
            var maxLength = lengths.Max();
            int expectedResultCount = keyLen.Length * mode.Length * maxLength;

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count(), Is.EqualTo(expectedResultCount));
        }

        private MathDomain GetMathDomainFromArray(int[] values)
        {
            MathDomain md = new MathDomain();

            foreach (var value in values)
            {
                md.AddSegment(new ValueDomainSegment(value));
            }

            return md;
        }
    }
}
