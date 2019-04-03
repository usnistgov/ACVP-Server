using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorKnownAnswerTests
    {
        private TestGroupGeneratorKnownAnswer _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorKnownAnswer();
        }

        [Test]
        // 0
        [TestCase(
            "test1 - 0",
            new string[] { },
            new int[] { },
            0
        )]
        // 0
        [TestCase(
            "test2 - 0",
            new string[] { },
            new int[] { 2 },
            0
        )]
        // 5 (1*5) // 1 directions, 5 kat types
        [TestCase(
            "test3 - 5",
            new string[] { "decrypt" },
            new int[] { 1 },
            5
        )]
        // 5 (1*5) // 1 directions, 5 kat types
        [TestCase(
            "test4 - 5",
            new string[] { "encrypt" },
            new int[] { 1 },
            5
        )]
        // 10 (2*5) // 2 directions, 5 kat types
        [TestCase(
            "test5 - 10",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1 },
            10
        )]
        // 10 (2*5) - 2 directions, 5 kat types, keying option parameter makes no difference to kats
        [TestCase(
            "test6 - 10",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1, 2 },
            10
        )]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string label,
            string[] mode,
            int[] keyOption,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "TDES-CFBP1",
                Revision = "1.0",
                Direction = mode,
                KeyingOption = keyOption,
            };

            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
