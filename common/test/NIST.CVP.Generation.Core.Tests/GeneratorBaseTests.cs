using System.IO;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class GeneratorBaseTests
    {
        private string _testPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\GeneratorBaseTests\");
            Directory.CreateDirectory(_testPath);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void ShouldProperlySaveOutputsForEachResolverWithValidFiles()
        {
            var subject = new FakeGeneratorBase();
            var testVectorSet = new FakeTestVectorSet();
            var result = subject.SaveOutputsTester(_testPath, testVectorSet);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldNotSaveOutputsForEachResolverWithInvalidPath()
        {
            var subject = new FakeGeneratorBase();
            var testVectorSet = new FakeTestVectorSet();
            var jsonPath = Path.Combine(_testPath, "fakePath/");
            var result = subject.SaveOutputsTester(jsonPath, testVectorSet);
            Assert.IsFalse(result.Success);
        }
    }
}