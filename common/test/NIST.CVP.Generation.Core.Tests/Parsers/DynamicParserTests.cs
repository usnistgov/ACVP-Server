using System;
using System.IO;
using NIST.CVP.Generation.Core.Parsers;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Parsers
{
    [TestFixture]
    public class DynamicParserTests
    {
        string _unitTestPath = Path.GetFullPath(@"..\..\TestFiles");

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = GetSubject();
            var result = subject.Parse($"C:\\{Guid.NewGuid()}\\testResults.json");
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
