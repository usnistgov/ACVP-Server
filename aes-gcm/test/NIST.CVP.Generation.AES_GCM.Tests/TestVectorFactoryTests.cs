using NIST.CVP.Generation.AES_GCM;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {
        [Test]
        // 0
        [TestCase(
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
                ivLen = new int[] { 1 },
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
