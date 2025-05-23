﻿using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFBP
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMultiblockMessageTests
    {
        private TestGroupGeneratorMultiblockMessage _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorMultiblockMessage();
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
        // 2 (2*1)
        [TestCase(
            "test3 - 2",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1 },
            2
        )]
        // 3 (2 * 2) - 1 for "keyingOption 2, encrypt" which is invalid.
        [TestCase(
            "test4 - 3",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1, 2 },
            3
        )]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string label,
            string[] mode,
            int[] keyOption,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "ACVP-TDES-CFBP1",
                Revision = "1.0",
                Direction = mode,
                KeyingOption = keyOption,
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count, Is.EqualTo(expectedResultCount));
        }

        [Test]
        public async Task ShouldNotReturnEncryptKeyingOption2Group()
        {
            Parameters p = new Parameters()
            {
                Algorithm = "ACVP-TDES-CFBP1",
                Revision = "1.0",
                Direction = new[] { "encrypt", "decrypt" },
                KeyingOption = new[] { 1, 2 },
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Any(a => a.Function.ToLower() == "encrypt" && a.KeyingOption == 2), Is.False);
        }
    }
}
