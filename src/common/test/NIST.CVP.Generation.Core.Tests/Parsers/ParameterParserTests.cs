﻿using System.IO;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class ParameterParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void SetUp()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = new ParameterParser<IParameters>();
            var result = subject.Parse(@"path/does/not/exist/file.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new ParameterParser<IParameters>();
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnErrorForBadFile()
        {
            var subject = new ParameterParser<IParameters>();
            var path = Path.Combine(_unitTestPath, "notJson.json");
            var result = subject.Parse(path);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }
    }
}