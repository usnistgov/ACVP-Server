using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.AES_CBC_CTS.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMultiBlockMessageTests
    {
        private TestGroupGeneratorMultiBlockMessagePartialBlock _subject;

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
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string label,
            string[] mode,
            int[] keyLen
        )
        {
            Parameters p = new ParameterBuilder()
                .WithKeyLen(keyLen)
                .WithMode(mode)
                .Build();
            int expectedResultCount = keyLen.Length * mode.Length;

            _subject = new TestGroupGeneratorMultiBlockMessagePartialBlock();
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
