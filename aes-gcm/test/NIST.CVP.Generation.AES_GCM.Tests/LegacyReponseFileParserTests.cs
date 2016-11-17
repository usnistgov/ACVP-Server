using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class LegacyReponseFileParserTests
    {
     
        private string _unitTestPath = @"C:\Users\def2\Documents\UnitTests\ACAVP\Standard";



        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse($"C:\\{Guid.NewGuid()}\\testResults.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new LegacyResponseFileParser();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = new LegacyResponseFileParser();
            var path = Path.Combine(_unitTestPath, "answer.json");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }
    }
}
