using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.AES_OFB.v1_0;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMultiBlockMessageTests
    {
        private TestGroupGeneratorMultiBlockMessage _subject;

        [Test]
        // 0
        [TestCase(
            "test1 - 0",
            new string[] { },
            new int[] { }
        )]
        // 0
        [TestCase(
            "test2 - 0",
            new string[] { },
            new int[] { 1 }
        )]
        // 3 (3*1)
        [TestCase(
            "test3 - 3",
            new string[] { "", "", "" },
            new int[] { 1 }
        )]
        // 9 (3*3)
        [TestCase(
            "test4 - 9",
            new string[] { "", "", "" },
            new int[] { 1, 2, 3 }
        )]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string label,
            string[] mode,
            int[] keyLen
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-ECB",
                KeyLen = keyLen,
                Direction = mode,
            };
            int expectedResultCount = keyLen.Length * mode.Length;

            _subject = new TestGroupGeneratorMultiBlockMessage();
            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
