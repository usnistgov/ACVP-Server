using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.Fakes;
using NuGet.Common;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture]
    public class TestCaseGeneratorFactoryFactoryTests
    {
        private Mock<IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>> _staticTestCaseGeneratorFactory;
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _testCaseGeneratorFactory;
        private TestCaseGeneratorFactoryFactory _subject;
        private TestVectorSet _testVectorSet;

        [SetUp]
        public void Setup()
        {
            _staticTestCaseGeneratorFactory = new Mock<IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>>();
            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _subject = new TestCaseGeneratorFactoryFactory(_testCaseGeneratorFactory.Object, _staticTestCaseGeneratorFactory.Object);
            _testVectorSet = new TestVectorSet()
            {
                Algorithm = "",
                TestGroups = new List<ITestGroup>()
                {
                    new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = "",
                        KeyLength = 128,
                        StaticGroupOfTests = true
                    },
                    new TestGroup()
                    {
                        Function = "encrypt",
                        TestType = "",
                        KeyLength = 128,
                        StaticGroupOfTests = false
                    },
                }
            };

            _staticTestCaseGeneratorFactory
                .Setup(s => s.GetStaticCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new FakeStaticTestCaseGenerator<TestGroup, TestCase>());
            _testCaseGeneratorFactory.Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new FakeTestCaseGenerator<TestGroup, TestCase>());
        }

        [Test]
        public void ShouldReturnErrorMessageWithinGenerateResponseWhenStaticFails()
        {
            _staticTestCaseGeneratorFactory
                .Setup(s => s.GetStaticCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new KnownAnswerTestCaseGeneratorNull());

            var result = _subject.BuildTestCases(_testVectorSet);
            
            Assert.IsFalse(result.Success, nameof(result.Success));
            Assert.IsTrue(!string.IsNullOrEmpty(result.ErrorMessage), nameof(result.ErrorMessage));
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
            _staticTestCaseGeneratorFactory
                .Verify(v => v.GetStaticCaseGenerator(
                        It.IsAny<TestGroup>()), 
                        Times.AtLeastOnce(), 
                        nameof(_staticTestCaseGeneratorFactory.Object.GetStaticCaseGenerator)
                );
            _testCaseGeneratorFactory
                .Verify(v => v.GetCaseGenerator(
                        It.IsAny<TestGroup>()),
                        Times.AtLeastOnce(),
                        nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator)
                );
        }
    }
}
