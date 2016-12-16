using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        [Test]
        // 0
        [TestCase(
            new string[] { },
            new int[] { },
            new int[] { }
        )]
        // 0
        [TestCase(
            new string[] { },
            new int[] { 1 },
            new int[] { 1, 2 }
        )]
        // 3 (3*1*1)
        [TestCase(
            new string[] { "", "", "" },
            new int[] { 1 },
            new int[] { 1 }
        )]
        // 27 (3*3*3)
        [TestCase(
            new string[] { "", "", "" },
            new int[] { 1, 2, 3 },
            new int[] { 1, 2, 3 }

        )]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string[] mode,
            int[] keyLen,
            int[] ptLen
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-ECB",
                KeyLen = keyLen,
                Mode = mode,
                PtLen = ptLen,
               
            };
            int expectedResultCount =  keyLen.Length * mode.Length * ptLen.Length;

            TestVectorFactory sut = new TestVectorFactory();
            var result = sut.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-ECB",
                KeyLen = new int[] { 1 },
                Mode = new [] {""},
                PtLen = new int[] { 1 },
                IsSample = isSample
            };
         

            TestVectorFactory sut = new TestVectorFactory();
            var result = sut.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

    }
}
