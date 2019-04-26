using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLineParser.Exceptions;
using CommandLineParser.Validation;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class ArgumentParsingHelperTests
    {
        private ArgumentParsingHelper _subject;
        private string _directory;

        [SetUp]
        public void SetUp()
        {
            _subject = new ArgumentParsingHelper();
            _directory = Utilities.GetConsistentTestingStartPath(GetType(), "../../testFiles");
        }

        [Test]
        public void ShouldParseGeneratorArgumentsCorrectly()
        {
            var registration = Path.Combine(_directory, "registration.json");
            var args = new[] {"-a", "algo", "-m", "mode", "-R", "1.0", "-g", registration};
            var result = _subject.Parse(args);
            
            Assert.AreEqual("algo", result.Algorithm, nameof(result.Algorithm));
            Assert.AreEqual("mode", result.Mode, nameof(result.Mode));
            Assert.AreEqual("1.0", result.Revision, nameof(result.Revision));
            Assert.AreEqual(registration, result.RegistrationFile.ToString(), nameof(result.RegistrationFile));
        }

        [Test]
        public void ShouldParseValidatorArgumentsCorrectly()
        {
            var answer = Path.Combine(_directory, "answer.json");
            var response = Path.Combine(_directory, "response.json");
            var args = new[] {"-a", "algo", "-m", "mode", "-R", "1.0", "-n", answer, "-r", response};
            var result = _subject.Parse(args);

            Assert.AreEqual("algo", result.Algorithm, nameof(result.Algorithm));
            Assert.AreEqual("mode", result.Mode, nameof(result.Mode));
            Assert.AreEqual(answer, result.AnswerFile.ToString(), nameof(result.AnswerFile));
            Assert.AreEqual(response, result.ResponseFile.ToString(), nameof(result.ResponseFile));
        }

        [Test]
        public void ShouldNotParseIncorrectArguments()
        {
            var registration = Path.Combine(_directory, "registration.json");
            var answer = Path.Combine(_directory, "answer.json");
            var response = Path.Combine(_directory, "response.json");

            var args = new[] {"-a", "algo", "-m", "mode", "-R", "1.0", "-g", registration, "-n", answer, "-r", response};
            Assert.Throws<ArgumentConflictException>(() => _subject.Parse(args));
        }
    }
}
