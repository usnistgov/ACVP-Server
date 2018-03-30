using System;
using System.IO;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class DynamicParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = GetSubject();
            var result = subject.Parse($@"C:\{Guid.NewGuid()}\testResults.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = GetSubject();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorForBadFile()
        {
            var subject = GetSubject();
            var path = Path.Combine(_unitTestPath, "notJson.json");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = GetSubject();
            var path = Path.Combine(_unitTestPath, "answer.json");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        private DynamicParser GetSubject()
        {
            return new DynamicParser();

        }
    }
}
