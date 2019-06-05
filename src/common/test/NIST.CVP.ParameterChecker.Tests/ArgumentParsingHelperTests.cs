using CommandLineParser.Exceptions;
using NIST.CVP.ParameterChecker.Helpers;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;

namespace NIST.CVP.ParameterChecker.Tests
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
            var args = new[] { "-r", registration };
            var result = _subject.Parse(args);

            Assert.AreEqual(registration, result.ParameterFile.ToString(), nameof(result.ParameterFile));
        }

        [Test]
        public void ShouldNotParseIncorrectArguments()
        {
            var registration = Path.Combine(_directory, "registration.json");
            var answer = Path.Combine(_directory, "answer.json");
            var response = Path.Combine(_directory, "response.json");

            var args = new[] { "-r", registration, "-n", answer, "-r", response };
            Assert.Throws<UnknownArgumentException>(() => _subject.Parse(args));
        }
    }
}
