using System;
using System.Linq;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
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

        private static object[] testData = new[]
        {
            new object[]
            {
                // 0
                "test1 - 0",
                new string[] { },
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new int[] { }
            },
            new object[]
            {
                // 3 (2*1*1*1)
                "test2 - 3",
                new string[] {"gen", "ver"},
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new int[] { 128 }
            },
            new object[]
            {
                "test3 - 1620",
                new string[] {"gen", "ver"},
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                new int[] { }
            }
        };
        
        [Test]
        [TestCaseSource(nameof(testData))]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            string[] mode,
            MathDomain msgLen,
            MathDomain macLen,
            int[] keyLen
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC-AES",
                Direction = mode,
                MsgLen = msgLen,
                MacLen = macLen,
                KeyLen = keyLen
            };

            var result = _subject.BuildTestGroups(p);

            var expectedResultCount = mode.Length * _subject.MsgLens.Length * _subject.MacLens.Length * keyLen.Length;

            Assert.AreEqual(expectedResultCount, result.Count());
        }
    }
}
