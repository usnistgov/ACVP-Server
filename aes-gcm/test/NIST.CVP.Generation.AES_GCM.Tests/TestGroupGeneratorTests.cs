using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

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
        // 0
        [TestCase(
            "test1 - 0",
            new string[] { },
            new int[] { },
            new int[] { },
            new int[] { },
            "",
            "",
            new int[] { },
            new int[] { }
        )]
        // 0
        [TestCase(
            "test2 - 0",
            new string[] { },
            new int[] { 1 },
            new int[] { 1, 2 },
            new int[] { 1, 2, 3 },
            "",
            "",
            new int[] { },
            new int[] { }
        )]
        // 3 (3*1*1*1*1*1)
        [TestCase(
            "test3 - 3",
            new string[] { "", "", "" },
            new int[] { 1 },
            new int[] { 1 },
            new int[] { 1 },
            "",
            "",
            new int[] { 1 },
            new int[] { 1 }
        )]
        // 1620 (3*3*3*3*4*5)
        [TestCase(
            "test4 - 1620",
            new string[] { "", "", "" },
            new int[] { 1, 2, 3 },
            new int[] { 1, 2, 3 },
            new int[] { 1, 2, 3 },
            "",
            "",
            new int[] { 1, 2, 3, 4 },
            new int[] { 1, 2, 3, 4, 5 }
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
            Parameters p = new Parameters()
            {
                aadLen = aadLen,
                Algorithm = "AES GCM",
                ivGen = ivGen,
                ivGenMode = ivGenMode,
                ivLen = ivLen,
                KeyLen = keyLen,
                Mode = mode,
                PtLen = ptLen,
                TagLen = tagLen
            };
            int expectedResultCount = aadLen.Length * ivLen.Length * keyLen.Length * mode.Length * ptLen.Length * tagLen.Length;

            var result = _subject.BuildTestGroups(p);

            Assert.AreEqual(expectedResultCount, result.Count());
        }
    }
}
