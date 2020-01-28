using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NIST.CVP.Generation.GenValApp.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using Autofac;
using Moq;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class GenValRunnerTests
    {
        private class FakeGenValRunner : GenValRunner
        {
            public FakeGenValRunner(IComponentContext scope) : base(scope)
            {
                var fileService = new Mock<IFileService>();
                fileService.Setup(s => s.ReadFile(It.IsAny<string>())).Returns(string.Empty);
                FileService = fileService.Object;
            }
        }
        
        private FakeGenValRunner _subject;
        private readonly FakeAutofacConfig _fakeAutofac = new FakeAutofacConfig();

        [Test]
        public void ShouldNotRunWithoutSettingRunningMode()
        {
            var parameters = new ArgumentParsingTarget();

            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.Run(parameters, GenValMode.Unset);

            Assert.AreNotEqual(0, result);
        }

        [Test]
        public void ShouldRunGeneration()
        {
            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.RunGeneration("registration.json");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(GenerateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldRunValidation()
        {
            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.RunValidation("response.json", "answer.json", false);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ValidateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(GenValMode.Generate, "registration.json", null, null, StatusCode.Success)]
        [TestCase(GenValMode.Validate, null, "response.json", "answer.json", StatusCode.Success)]
        [TestCase(GenValMode.Unset, null, null, null, StatusCode.ModeError)]
        public void ShouldRun(GenValMode genValMode, string registrationFile, string responseFile, string answerFile, int returnCode)
        {
            var parameters = new ArgumentParsingTarget
            {
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo(registrationFile) : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo(responseFile) : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo(answerFile) : null
            };

            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.Run(parameters, genValMode);

            Assert.AreEqual(returnCode, result);
        }
    }
}
