using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using Moq;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryFactoryTests
    {
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _testCaseGeneratorFactory;
        private TestCaseGeneratorFactoryFactory _subject;
        private TestVectorSet _testVectorSet;

        [SetUp]
        public void Setup()
        {
            _testCaseGeneratorFactory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _subject = new TestCaseGeneratorFactoryFactory(_testCaseGeneratorFactory.Object);
            _testVectorSet = new TestVectorSet
            {
                TestGroups = new List<ITestGroup>
                {
                    new TestGroup
                    {
                        Direction = "encrypt",
                        KeyLen = 128,
                        TweakMode = "hex",
                        PtLen = 128
                    },
                    new TestGroup
                    {
                        Direction = "encrypt",
                        KeyLen = 128,
                        TweakMode = "number",
                        PtLen = 128
                    }
                }
            };

            _testCaseGeneratorFactory.Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(new FakeTestCaseGenerator<TestGroup, TestCase>());
        }

        [Test]
        public void ShouldReturnGenerateResponseWhenGenerateSuccess()
        {
            var results = _subject.BuildTestCases(_testVectorSet);

            Assert.IsTrue(results.Success);
            _testCaseGeneratorFactory
                .Verify(v => v.GetCaseGenerator(
                        It.IsAny<TestGroup>()),
                        Times.AtLeastOnce(),
                        nameof(_testCaseGeneratorFactory.Object.GetCaseGenerator)
                );
        }
    }
}
