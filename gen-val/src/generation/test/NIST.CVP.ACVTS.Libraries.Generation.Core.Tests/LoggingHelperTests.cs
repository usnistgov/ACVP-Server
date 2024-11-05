using System;
using System.IO;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests
{
    [TestFixture, UnitTest]
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
            //Assert.That(lltrue, Is.True);
        }

        [Test]
        [TestCase(@"X:\Fruzengaz\fred.doc")]
        [TestCase(@"X:\Fruzengaz\fred.json")]
        [TestCase(@"C:\Windows\fred.json")]
        public void ShouldConfigureLoggingEvenWithInvalidFilepath(string path)
        {
            LoggingHelper.ConfigureLogging(path, "DNA");
            //Assert.That(true, Is.True);
        }

        [Test]
        public void ShouldConfigureLoggingWithValidFilepath()
        {
            LoggingHelper.ConfigureLogging(_testFile, "DNA");
            //Assert.That(true, Is.True);
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
