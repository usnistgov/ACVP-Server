using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        [Test]
        // 0
        [TestCase(
            new string[] { },
            new int[] { }
        )]
        // 0
        [TestCase(
            new string[] { },
            new int[] { 1 }
        )]
        // 3 (3*1)
        [TestCase(
            new string[] { "", "", "" },
            new int[] { 1 }
        )]
        // 9 (3*3)
        [TestCase(
            new string[] { "", "", "" },
            new int[] { 1, 2, 3 }

        )]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string[] mode,
            int[] keyLen
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-ECB",
                KeyLen = keyLen,
                Mode = mode
            };
            int expectedResultCount =  keyLen.Length * mode.Length;

            Mock<IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>>> iKATTestGroupFactory = new Mock<IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iKATTestGroupFactory
                .Setup(s => s.BuildKATTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>> iMCTTestGroupFactory = new Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iMCTTestGroupFactory
                .Setup(s => s.BuildMCTTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            TestVectorFactory subject = new TestVectorFactory(iKATTestGroupFactory.Object, iMCTTestGroupFactory.Object);
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
                Algorithm = "AES-ECB",
                KeyLen = new int[] { 1 },
                Mode = new [] {""},
                IsSample = isSample
            };

            Mock<IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>>> iKATTestGroupFactory = new Mock<IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iKATTestGroupFactory
                .Setup(s => s.BuildKATTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>> iMCTTestGroupFactory = new Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iMCTTestGroupFactory
                .Setup(s => s.BuildMCTTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            TestVectorFactory subject = new TestVectorFactory(iKATTestGroupFactory.Object, iMCTTestGroupFactory.Object);
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

    }
}
