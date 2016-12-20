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
            string path = Path.Combine(_unitTestPath, "badPath.json");
            ConfigParser configParser = new ConfigParser(path);
            Assert.IsNotNull(configParser.success);
            Assert.IsFalse(configParser.success);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            string path = $@"C:\{Guid.NewGuid()}\exampleConfig.json";
            ConfigParser configParser = new ConfigParser(path);
            Assert.IsNotNull(configParser.success);
            Assert.IsFalse(configParser.success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            ConfigParser configParser = new ConfigParser(path);
            Assert.IsNotNull(configParser.success);
            Assert.IsFalse(configParser.success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            string path = Path.Combine(_unitTestPath, "exampleConfig.json");
            ConfigParser configParser = new ConfigParser(path);
            Assert.IsNotNull(configParser.success);
            Assert.IsTrue(configParser.success);
        }

        [Test]
        public void ShouldReadCorrectValuesFromFields()
        {
            string path = Path.Combine(_unitTestPath, "exampleConfig.json");
            ConfigParser configParser = new ConfigParser(path);
            Assume.That(configParser.success);
            Assert.AreEqual(10, int.Parse(configParser.Configuration["testField"]));
            Assert.AreEqual(20, int.Parse(configParser.Configuration["secondField"]));
        }

        [Test]
        public void ShouldReadCorrectValuesFromArrays()
        {
            string path = Path.Combine(_unitTestPath, "exampleConfig.json");
            ConfigParser configParser = new ConfigParser(path);
            Assume.That(configParser.success);
            Assert.AreEqual(30, int.Parse(configParser.Configuration["arrayField:0"]));
            Assert.AreEqual(40, int.Parse(configParser.Configuration["arrayField:1"]));
        }

        [Test]
        public void ShouldReadCorrectValuesFromObjects()
        {
            string path = Path.Combine(_unitTestPath, "exampleConfig.json");
            ConfigParser configParser = new ConfigParser(path);
            Assume.That(configParser.success);
            Assert.AreEqual(50, int.Parse(configParser.Configuration["objectField:insideField"]));
        }
    }
}
