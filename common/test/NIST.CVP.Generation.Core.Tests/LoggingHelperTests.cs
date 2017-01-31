using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class LoggingHelperTests
    {
        private const string _WORKING_PATH = @"C:\temp";
        private string _testFile = null;
        [OneTimeSetUp]
        public void Setup()
        {
            if (!Directory.Exists(_WORKING_PATH))
            {
                Directory.CreateDirectory(_WORKING_PATH);
            }
            _testFile = Path.Combine(_WORKING_PATH, $"{Guid.NewGuid()}.json");
            File.WriteAllText(_testFile, "hi");
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldConfigureLoggingEvenWithNoFilepath(string path)
        {
            LoggingHelper.ConfigureLogging(path, "DNA");
            Assert.IsTrue(true);
        }

        [Test]
        [TestCase(@"X:\Fruzengaz\fred.doc")]
        [TestCase(@"X:\Fruzengaz\fred.json")]
        [TestCase(@"C:\Windows\fred.json")]
        public void ShouldConfigureLoggingEvenWithInvalidFilepath(string path)
        {
            LoggingHelper.ConfigureLogging(path, "DNA");
            Assert.IsTrue(true);
        }

        [Test]
        public void ShouldConfigureLoggingWithValidFilepath()
        { 
            LoggingHelper.ConfigureLogging(_testFile, "DNA");
            Assert.IsTrue(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (File.Exists(_testFile))
            {
                File.Delete(_testFile);
            }
        }
    }
}
