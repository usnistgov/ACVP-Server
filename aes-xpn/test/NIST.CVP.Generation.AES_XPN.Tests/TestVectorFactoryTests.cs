using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {
        [Test]
        // 0
        [TestCase(
            "test1 - 0",
            new string[] { },
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
            "",
            "",
            new int[] { },
            new int[] { }
        )]
        // 3 (3*1*1*1*1)
        [TestCase(
            "test3 - 3",
            new string[] { "", "", "" },
            new int[] { 1 },
            new int[] { 1 },
            "",
            "",
            new int[] { 1 },
            new int[] { 1 }
        )]
        // 540 (3*3*3*4*5)
        [TestCase(
            "test4 - 1620",
            new string[] { "", "", "" },
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
                KeyLen = keyLen,
                Mode = mode,
                PtLen = ptLen,
                TagLen = tagLen
            };
            int expectedResultCount = aadLen.Length * keyLen.Length * mode.Length * ptLen.Length * tagLen.Length;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            Parameters p = new Parameters()
            {
                aadLen = new int[] { 1 },
                Algorithm = "AES GCM",
                ivGen = "",
                ivGenMode = "",
                KeyLen = new int[] { 1 },
                Mode = new [] {""},
                PtLen = new int[] { 1 },
                TagLen = new int[] { 1 },
                IsSample = isSample
            };
         
            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

    }
}
