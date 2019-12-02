using NIST.CVP.Generation.AES_GCM.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.AES_GCM.Tests
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
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
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
            MathDomain mdTag = GetMathDomainFromArray(tagLen);

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
                TagLen = mdTag
            };
            int expectedResultCount = aadLen.Length * ivLen.Length * keyLen.Length * mode.Length * ptLen.Length * tagLen.Length;

            var result = _subject.BuildTestGroups(p);

            Assert.AreEqual(expectedResultCount, result.Count());
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
