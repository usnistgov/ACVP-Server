using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.Fakes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryFactoryTests
    {
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _testCaseGeneratorFactory;
        private TestCaseGeneratorFactoryFactory _subject;
        private TestVectorSet _testVectorSet;

        [SetUp]
        public void SetUp()
        {
            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _subject = new TestCaseGeneratorFactoryFactory(_testCaseGeneratorFactory.Object);
            _testVectorSet = new TestVectorSet()
            {
                Algorithm = "",
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        TestType = "",
                        DigestLength = 160,
                        BitOriented = false
                    },
                    new TestGroup()
                    {
                        TestType = "",
                        DigestLength = 160,
                        BitOriented = true
                    }
                }
            };

            _testCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new FakeTestCaseGenerator<TestGroup, TestCase>());
        }

        [Test]
        public void ShouldReturnErrorMessageWithinGenerateResponseWhenFails()
        {
            _testCaseGeneratorFactory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new TestCaseGeneratorNull());

            var result = _subject.BuildTestCases(_testVectorSet);

            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage), nameof(result.ErrorMessage));
        }

        [Test]
        public void ShouldReturnGenerateResponseWhenGenerateSuccess()
        {
            var results = _subject.BuildTestCases(_testVectorSet);

            Assert.IsTrue(results.Success);

            _testCaseGeneratorFactory
                .Verify(v => v.GetCaseGenerator(It.IsAny<TestGroup>()), Times.AtLeastOnce(), nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator));
        }
    }
}
