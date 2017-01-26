using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NIST.CVP.Generation.Core;
using Moq;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestVectorFactoryTests
    {
        [Test]
        [TestCase(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1 }, new int[] { 1 })]
        [TestCase(new int[] { 1, 2 }, new int[] { 1, 2, 3 })]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParameters(
            int[] msgLen,
            int[] digLen
        )
        {
            Parameters parameters = new Parameters()
            {
                Algorithm = "SHA1",
                BitOriented = true,
                IncludeNull = true,
                DigestLen = digLen,
                MessageLen = msgLen
            };

            int expectedResultCount = msgLen.Length * digLen.Length;

            var iMCTTestGroupFactory = new Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iMCTTestGroupFactory
                .Setup(s => s.BuildMCTTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());

            TestVectorFactory subject = new TestVectorFactory(iMCTTestGroupFactory.Object);
            var result = subject.BuildTestVectorSet(parameters);

            Assert.AreEqual(expectedResultCount, result.TestGroups.Count);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            Parameters parameters = new Parameters()
            {
                Algorithm = "SHA1",
                BitOriented = true,
                IncludeNull = true,
                DigestLen = new int[] {1},
                MessageLen = new int[] {1},
                IsSample = isSample
            };

            var iMCTTestGroupFactory = new Mock<IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>>>();
            iMCTTestGroupFactory
                .Setup(s => s.BuildMCTTestGroups(It.IsAny<Parameters>()))
                .Returns(new List<TestGroup>());

            TestVectorFactory subject = new TestVectorFactory(iMCTTestGroupFactory.Object);
            var result = subject.BuildTestVectorSet(parameters);

            Assert.AreEqual(isSample, result.IsSample);
        }
    }
}
