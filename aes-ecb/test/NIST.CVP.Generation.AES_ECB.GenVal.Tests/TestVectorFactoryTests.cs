using System.Collections.Generic;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.GenVal.Tests
{
    [TestFixture, UnitTest]
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
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string[] mode,
            int[] keyLen
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-ECB",
                KeyLen = keyLen,
                Mode = mode,
            };
            int expectedResultCount =  keyLen.Length * mode.Length;
            Mock<IKnownAnswerTestGroupFactory<Parameters, TestGroup>> iKATTestGroupFactory = new Mock<IKnownAnswerTestGroupFactory<Parameters, TestGroup>>();
            iKATTestGroupFactory
                .Setup(s => s.BuildKATTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            Mock<IMonteCarloTestGroupFactory<Parameters, TestGroup>> iMCTTestGroupFactory = new Mock<IMonteCarloTestGroupFactory<Parameters, TestGroup>>();
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

            Mock<IKnownAnswerTestGroupFactory<Parameters, TestGroup>> iKATTestGroupFactory = new Mock<IKnownAnswerTestGroupFactory<Parameters, TestGroup>>();
            iKATTestGroupFactory
                .Setup(s => s.BuildKATTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            Mock<IMonteCarloTestGroupFactory<Parameters, TestGroup>> iMCTTestGroupFactory = new Mock<IMonteCarloTestGroupFactory<Parameters, TestGroup>>();
            iMCTTestGroupFactory
                .Setup(s => s.BuildMCTTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());
            TestVectorFactory subject = new TestVectorFactory(iKATTestGroupFactory.Object, iMCTTestGroupFactory.Object);
            var result = subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

    }
}
