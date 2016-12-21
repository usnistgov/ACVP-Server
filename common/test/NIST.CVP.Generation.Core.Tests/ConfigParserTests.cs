using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{

    [TestFixture]
    public class ConfigParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\ConfigFiles\");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentFile()
        {
            ConfigParser subject = getSubject("badFile.json");
            Assert.IsFalse(subject.success);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            string path = $@"C:\{Guid.NewGuid()}\exampleConfig.json";
            ConfigParser subject = new ConfigParser(path);
            Assert.IsFalse(subject.success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            ConfigParser subject = new ConfigParser(path);
            Assert.IsFalse(subject.success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            ConfigParser subject = getSubject("exampleConfig.json");
            Assert.IsTrue(subject.success);
        }

        [Test]
        [TestCase("testField", 10)]                 // Tests a regular field
        [TestCase("secondField", 20)]               // ''
        [TestCase("arrayField:0", 30)]              // Tests a field that holds an array
        [TestCase("arrayField:1", 40)]              // ''
        [TestCase("objectField:insideField", 50)]    // Tests an object that has a field inside
        public void ShouldReadCorrectValuesFromFields(string field, int value)
        {
            ConfigParser subject = getSubject("exampleConfig.json");
            Assume.That(subject.success);
            Assert.AreEqual(value, int.Parse(subject.Configuration[field]));
        }

        private ConfigParser getSubject(string fileName)
        {
            string path = Path.Combine(_unitTestPath, fileName);
            return new ConfigParser(path);
        }
    }
}
