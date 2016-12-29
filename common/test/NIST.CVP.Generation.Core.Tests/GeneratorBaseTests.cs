using System.Collections.Generic;
using System.IO;
using Moq;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class GeneratorBaseTests
    {
        private string _testPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\GeneratorBaseTests\");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        [TestCase("answer")]
        [TestCase("prompt")]
        [TestCase("result")]
        [Ignore("Incomplete")]
        public void ShouldProperlySaveOutputsForEachResolverWithValidFiles(string resolverType)
        {
            var subject = new FakeGeneratorBase(resolverType);
            var jsonPath = Path.Combine(_testPath, $"{resolverType}output.json");
            var testVectorSet = new FakeTestVectorSet();
            var result = subject.SaveOutputsTester(jsonPath, testVectorSet);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("answer")]
        [TestCase("prompt")]
        [TestCase("result")]
        [Ignore("Incomplete")]
        public void ShouldNotSaveOutputsForEachResolverWithInvalidFiles(string resolverType)
        {
            var subject = new FakeGeneratorBase(resolverType);
            var jsonPath = Path.Combine(_testPath, $"{resolverType}Output.json");
            var testVectorSet = new FakeTestVectorSet();
            var result = subject.SaveOutputsTester(jsonPath, testVectorSet);
            Assert.IsFalse(result.Success);
        }
    }
}