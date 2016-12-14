using System;
using System.IO;
using System.Reflection;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests.Parsers
{
    [TestFixture]
    public class DynamicParserTests
    {
        string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles");
        }

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
            Assert.IsTrue(result.Success, _unitTestPath);
        }

        private DynamicParser GetSubject()
        {
            return new DynamicParser();
        }
    }
}
