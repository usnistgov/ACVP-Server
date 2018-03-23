using System.Collections.Generic;
using Moq;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using FakeParameters = NIST.CVP.Generation.Core.Tests.Fakes.FakeParameters;
using FakeTestVectorSet = NIST.CVP.Generation.Core.Tests.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {

        private TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> _subject;
        private Mock<ITestGroupGeneratorFactory<FakeParameters, FakeTestGroup, FakeTestCase>> _testGroupGeneratorFactory;
        private Mock<ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>> _testGroupGenerator;

        [SetUp]
        public void Setup()
        {
            _testGroupGeneratorFactory = new Mock<ITestGroupGeneratorFactory<FakeParameters, FakeTestGroup, FakeTestCase>>();
            _testGroupGenerator = new Mock<ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>>();
    }

        [Test]
        [TestCase("test")]
        [TestCase("test-again")]
        public void ShouldSetAlgorithmProperlyFromTheParameters(string algorithm)
        {
            FakeParameters p = new FakeParameters()
            {
                Algorithm = algorithm,
                IsSample = true
            };

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);
            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(algorithm, result.Algorithm);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            FakeParameters p = new FakeParameters()
            {
                Algorithm = "anAlgorithm",
                IsSample = isSample
            };

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);
            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldInvokeITestGroupGeneratorExpectedNumberOfTimes(int numberOfInvokes)
        {
            List<ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>> gennies = new List<ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>>();
            for (int i = 0; i < numberOfInvokes; i++)
            {
                gennies.Add(_testGroupGenerator.Object);
            }

            _testGroupGeneratorFactory
                .Setup(s => s.GetTestGroupGenerators())
                .Returns(gennies);
            _testGroupGenerator
                .Setup(s => s.BuildTestGroups(It.IsAny<FakeParameters>()))
                .Returns(new List<FakeTestGroup>());

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);

            FakeParameters p = new FakeParameters();
            _subject.BuildTestVectorSet(p);

            _testGroupGeneratorFactory.Verify(v => v.GetTestGroupGenerators(), Times.Once, nameof(_subject.BuildTestVectorSet));
            _testGroupGenerator.Verify(v => v.BuildTestGroups(It.IsAny<FakeParameters>()), Times.Exactly(numberOfInvokes), nameof(_testGroupGenerator.Object.BuildTestGroups));
        }

        [Test]
        [TestCase(null, "AFT")]
        [TestCase("", "AFT")]
        [TestCase("aft", "aft")]
        [TestCase("AFT", "AFT")]
        [TestCase("MCT", "MCT")]
        public void ShouldSetAftInGroupWhenNullOrEmpty(string testType, string expectedType)
        {
            var gennies = new List<ITestGroupGenerator<FakeParameters, FakeTestGroup, FakeTestCase>>
            {
                _testGroupGenerator.Object
            };

            _testGroupGeneratorFactory
                .Setup(s => s.GetTestGroupGenerators())
                .Returns(gennies);
            _testGroupGenerator
                .Setup(s => s.BuildTestGroups(It.IsAny<FakeParameters>()))
                .Returns(() => 
                    new List<FakeTestGroup>()
                    {
                        new FakeTestGroup()
                        {
                            TestType = testType
                        }
                    });

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);

            FakeParameters p = new FakeParameters();

            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(result.TestGroups[0].TestType, expectedType);
        }
    }
}
