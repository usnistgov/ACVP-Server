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
                Algorithm = "AES-XPN",
                ivGen = ivGen,
                ivGenMode = ivGenMode,
                KeyLen = keyLen,
                Direction = mode,
                PtLen = ptLen,
                TagLen = tagLen
            };
            int expectedResultCount = aadLen.Length * keyLen.Length * mode.Length * ptLen.Length * tagLen.Length;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count);
        }

        [Test]
        public void ShouldSetAadLen()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.aadLen[0], ((TestGroup)result.TestGroups[0]).AADLength);
        }

        [Test]
        public void ShouldSetAlgorithm()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.Algorithm, result.Algorithm);
        }

        [Test]
        public void ShouldSetIvGen()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.ivGen, ((TestGroup)result.TestGroups[0]).IVGeneration);
        }

        [Test]
        public void ShouldSetIvGenMode()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.ivGenMode, ((TestGroup)result.TestGroups[0]).IVGenerationMode);
        }

        [Test]
        public void ShouldSetDirection()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.Direction[0], ((TestGroup)result.TestGroups[0]).Function);
        }

        [Test]
        public void ShouldSetPtLen()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.PtLen[0], ((TestGroup)result.TestGroups[0]).PTLength);
        }

        [Test]
        public void ShouldSetTagLen()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.TagLen[0], ((TestGroup)result.TestGroups[0]).TagLength);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            Parameters p = GetParameters();
            p.IsSample = isSample;

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

        [Test]
        public void ShouldSetSaltGen()
        {
            Parameters p = GetParameters();

            TestVectorFactory subject = new TestVectorFactory();
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(p.SaltGen, ((TestGroup)result.TestGroups[0]).SaltGen);
        }

        private Parameters GetParameters()
        {
            return new Parameters()
            {
                aadLen = new int[] { 1 },
                Algorithm = "test0",
                ivGen = "test",
                ivGenMode = "test2",
                KeyLen = new int[] { 2 },
                Direction = new[] { "test3" },
                PtLen = new int[] { 3 },
                TagLen = new int[] { 4 },
                IsSample = false,
                SaltGen = "test4"
            };
        }
    }
}
