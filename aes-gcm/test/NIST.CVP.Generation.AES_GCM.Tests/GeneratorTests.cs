using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Generation.AES_GCM.Tests.Fakes;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        private string _testRoot = @"C:\Users\def2\Documents\UnitTests\ACAVP";
        private string _testPath;
        [OneTimeSetUp]
        public void Setup()
        {
            _testPath = Path.Combine(_testRoot, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testPath);
        }

        [Test]
        public void ShouldGenerateResults()
        {
            var subject = new Generator(new TestVectorFactory(), new ParameterParserFake(), new ParameterValidator(), new TestCaseGeneratorFactory(new Random800_90(), new AES_GCM()));
            var result = subject.Generate(Path.Combine(_testPath, "parameters.json"));
            Assert.IsTrue(result.Success);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
           Directory.Delete(_testPath, true);
        }
    }
}
