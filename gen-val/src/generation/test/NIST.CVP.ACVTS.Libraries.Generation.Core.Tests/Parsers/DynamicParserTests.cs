using System;
using System.IO;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Parsers;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class DynamicParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\DynamicParser");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = GetSubject();
            var result = subject.Parse(Path.Combine(_unitTestPath, $"{Guid.NewGuid()}", "testResults.json"));
            Assert.That(result, Is.Not.Null, nameof(result));
            Assert.That(result.Success, Is.False, nameof(result.Success));
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = GetSubject();
            var result = subject.Parse(path);
            Assert.That(result, Is.Not.Null, nameof(result));
            Assert.That(result.Success, Is.False, nameof(result.Success));
        }

        [Test]
        public void ShouldReturnErrorForBadFile()
        {
            var subject = GetSubject();
            var path = Path.Combine(_unitTestPath, "notJson.json");
            var result = subject.Parse(path);
            Assert.That(result, Is.Not.Null, nameof(result));
            Assert.That(result.Success, Is.False, nameof(result.Success));
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = GetSubject();
            var path = Path.Combine(_unitTestPath, "answer.json");
            var result = subject.Parse(path);
            Assert.That(result, Is.Not.Null, nameof(result));
            Assert.That(result.Success, Is.True, nameof(result.Success));
        }

        private DynamicParser GetSubject()
        {
            return new DynamicParser();
        }
    }
}
