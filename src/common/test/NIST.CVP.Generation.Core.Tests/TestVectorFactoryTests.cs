using System.Collections.Generic;
using Moq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using FakeParameters = NIST.CVP.Generation.Core.Tests.Fakes.FakeParameters;
using FakeTestVectorSet = NIST.CVP.Generation.Core.Tests.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {

        private TestVectorFactory<IParameters, FakeTestVectorSet> _subject;
        private Mock<ITestGroupGeneratorFactory<IParameters>> _testGroupGeneratorFactory;
        private Mock<ITestGroupGenerator<IParameters>> _testGroupGenerator;

        [SetUp]
        public void Setup()
        {
            _testGroupGeneratorFactory = new Mock<ITestGroupGeneratorFactory<IParameters>>();
            _testGroupGenerator = new Mock<ITestGroupGenerator<IParameters>>();
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

            _subject = new TestVectorFactory<IParameters, FakeTestVectorSet>(_testGroupGeneratorFactory.Object);
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

            _subject = new TestVectorFactory<IParameters, FakeTestVectorSet>(_testGroupGeneratorFactory.Object);
            var result = _subject.BuildTestVectorSet(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldInvokeITestGroupGeneratorExpectedNumberOfTimes(int numberOfInvokes)
        {
            List<ITestGroupGenerator<IParameters>> gennies = new List<ITestGroupGenerator<IParameters>>();
            for (int i = 0; i < numberOfInvokes; i++)
            {
                gennies.Add(_testGroupGenerator.Object);
            }

            _testGroupGeneratorFactory
                .Setup(s => s.GetTestGroupGenerators())
                .Returns(gennies);
            _testGroupGenerator
                .Setup(s => s.BuildTestGroups(It.IsAny<IParameters>()))
                .Returns(new List<ITestGroup>());

            _subject = new TestVectorFactory<IParameters, FakeTestVectorSet>(_testGroupGeneratorFactory.Object);

            FakeParameters p = new FakeParameters();
            _subject.BuildTestVectorSet(p);

            _testGroupGeneratorFactory.Verify(v => v.GetTestGroupGenerators(), Times.Once, nameof(_subject.BuildTestVectorSet));
            _testGroupGenerator.Verify(v => v.BuildTestGroups(It.IsAny<IParameters>()), Times.Exactly(numberOfInvokes), nameof(_testGroupGenerator.Object.BuildTestGroups));
        }
    }
}
