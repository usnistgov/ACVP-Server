using System;
using System.Collections.Generic;
using System.Text;
using CommandLineParser.Exceptions;
using CommandLineParser.Validation;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class ArgumentParsingHelperTests
    {
        private ArgumentParsingHelper _subject;
        private readonly string _directory = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\..\\..\\testFiles";

        [SetUp]
        public void SetUp()
        {
            _subject = new ArgumentParsingHelper();
        }

        [Test]
        public void ShouldParseGeneratorArgumentsCorrectly()
        {
            var registration = $"{_directory}\\registration.json";
            var args = new[] {"-a", "algo", "-m", "mode", "-d", _directory, "-g", registration};
            var result = _subject.Parse(args);
            
            Assert.AreEqual("algo", result.Algorithm, nameof(result.Algorithm));
            Assert.AreEqual("mode", result.Mode, nameof(result.Mode));
            Assert.AreEqual(_directory, result.DllLocation.ToString(), nameof(result.DllLocation));
            Assert.AreEqual(registration, result.RegistrationFile.ToString(), nameof(result.RegistrationFile));
        }

        [Test]
        public void ShouldParseValidatorArgumentsCorrectly()
        {
            var answer = $"{_directory}\\answer.json";
            var response = $"{_directory}\\response.json";
            var args = new[] {"-a", "algo", "-m", "mode", "-d", _directory, "-n", answer, "-r", response};
            var result = _subject.Parse(args);

            Assert.AreEqual("algo", result.Algorithm, nameof(result.Algorithm));
            Assert.AreEqual("mode", result.Mode, nameof(result.Mode));
            Assert.AreEqual(_directory, result.DllLocation.ToString(), nameof(result.DllLocation));
            Assert.AreEqual(answer, result.AnswerFile.ToString(), nameof(result.AnswerFile));
            Assert.AreEqual(response, result.ResponseFile.ToString(), nameof(result.ResponseFile));
        }

        [Test]
        public void ShouldNotParseIncorrectArguments()
        {
            var registration = $"{_directory}\\registration.json";
            var answer = $"{_directory}\\answer.json";
            var response = $"{_directory}\\response.json";

            var args = new[] {"-a", "algo", "-m", "mode", "-d", _directory, "-g", registration, "-n", answer, "-r", response};
            Assert.Throws<ArgumentConflictException>(() => _subject.Parse(args));
        }
    }
}
