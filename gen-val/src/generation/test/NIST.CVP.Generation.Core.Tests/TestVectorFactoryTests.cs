using Moq;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeParameters = NIST.CVP.Generation.Core.Tests.Fakes.FakeParameters;
using FakeTestVectorSet = NIST.CVP.Generation.Core.Tests.Fakes.FakeTestVectorSet;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {

        private TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase> _subject;
        private Mock<ITestGroupGeneratorFactory<FakeParameters, FakeTestGroup, FakeTestCase>> _testGroupGeneratorFactory;
        private Mock<ITestGroupGeneratorAsync<FakeParameters, FakeTestGroup, FakeTestCase>> _testGroupGenerator;

        [SetUp]
        public void Setup()
        {
            _testGroupGeneratorFactory = new Mock<ITestGroupGeneratorFactory<FakeParameters, FakeTestGroup, FakeTestCase>>();
            _testGroupGenerator = new Mock<ITestGroupGeneratorAsync<FakeParameters, FakeTestGroup, FakeTestCase>>();
    }

        [Test]
        [TestCase("test")]
        [TestCase("test-again")]
        public async Task ShouldSetAlgorithmProperlyFromTheParameters(string algorithm)
        {
            FakeParameters p = new FakeParameters()
            {
                Algorithm = algorithm,
                IsSample = true
            };

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);
            var result = await _subject.BuildTestVectorSetAsync(p);

            Assert.AreEqual(algorithm, result.Algorithm);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldSetIsSampleProperlyFromTheParameters(bool isSample)
        {
            FakeParameters p = new FakeParameters()
            {
                Algorithm = "anAlgorithm",
                IsSample = isSample
            };

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);
            var result = await _subject.BuildTestVectorSetAsync(p);

            Assert.AreEqual(isSample, result.IsSample);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task ShouldInvokeITestGroupGeneratorExpectedNumberOfTimes(int numberOfInvokes)
        {
            var gennies = new List<ITestGroupGeneratorAsync<FakeParameters, FakeTestGroup, FakeTestCase>>();
            for (int i = 0; i < numberOfInvokes; i++)
            {
                gennies.Add(_testGroupGenerator.Object);
            }

            _testGroupGeneratorFactory
                .Setup(s => s.GetTestGroupGenerators(It.IsAny<FakeParameters>()))
                .Returns(gennies);
            _testGroupGenerator
                .Setup(s => s.BuildTestGroupsAsync(It.IsAny<FakeParameters>()))
                .Returns(Task.FromResult(new List<FakeTestGroup>().AsEnumerable()));

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);

            FakeParameters p = new FakeParameters();
            await _subject.BuildTestVectorSetAsync(p);

            _testGroupGeneratorFactory.Verify(v => v.GetTestGroupGenerators(It.IsAny<FakeParameters>()), Times.Once, nameof(_subject.BuildTestVectorSetAsync));
            _testGroupGenerator.Verify(v => v.BuildTestGroupsAsync(It.IsAny<FakeParameters>()), Times.Exactly(numberOfInvokes), nameof(_testGroupGenerator.Object.BuildTestGroupsAsync));
        }

        [Test]
        [TestCase(null, "AFT")]
        [TestCase("", "AFT")]
        [TestCase("aft", "aft")]
        [TestCase("AFT", "AFT")]
        [TestCase("MCT", "MCT")]
        public async Task ShouldSetAftInGroupWhenNullOrEmpty(string testType, string expectedType)
        {
            var gennies = new List<ITestGroupGeneratorAsync<FakeParameters, FakeTestGroup, FakeTestCase>>
            {
                _testGroupGenerator.Object
            };

            _testGroupGeneratorFactory
                .Setup(s => s.GetTestGroupGenerators(It.IsAny<FakeParameters>()))
                .Returns(gennies);
            _testGroupGenerator
                .Setup(s => s.BuildTestGroupsAsync(It.IsAny<FakeParameters>()))
                .Returns(() => 
                    Task.FromResult(new List<FakeTestGroup>()
                    {
                        new FakeTestGroup()
                        {
                            TestType = testType
                        }
                    }.AsEnumerable()));

            _subject = new TestVectorFactory<FakeParameters, FakeTestVectorSet, FakeTestGroup, FakeTestCase>(_testGroupGeneratorFactory.Object);

            FakeParameters p = new FakeParameters();

            var result = await _subject.BuildTestVectorSetAsync(p);

            Assert.AreEqual(result.TestGroups[0].TestType, expectedType);
        }
    }
}
