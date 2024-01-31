using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Moq;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests.Fakes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests
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
        public async Task ShouldNotRunWithoutSettingRunningMode()
        {
            var parameters = new ArgumentParsingTarget();

            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = await _subject.Run(parameters, GenValMode.Unset);

            Assert.AreNotEqual(0, result);
        }

        [Test]
        public async Task ShouldRunGeneration()
        {
            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = await _subject.RunGeneration("registration.json");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(GenerateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public async Task ShouldRunValidation()
        {
            _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = await _subject.RunValidation("response.json", "answer.json", false);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ValidateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
//        [TestCase(GenValMode.Check, "registration.json", null, null, StatusCode.Success)]
        [TestCase(GenValMode.Generate, "registration.json", null, null, StatusCode.Success)]
        [TestCase(GenValMode.Validate, null, "response.json", "answer.json", StatusCode.Success)]
        [TestCase(GenValMode.Unset, null, null, null, StatusCode.ModeError)]
        public async Task ShouldRun(GenValMode genValMode, string registrationFile, string responseFile, string answerFile, int returnCode)
        {
            var parameters = new ArgumentParsingTarget
            {
  //              CapabilitiesFile = genValMode == GenValMode.Check ? new FileInfo(registrationFile) : null,
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo(registrationFile) : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo(responseFile) : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo(answerFile) : null
            };

            var result = int.MinValue;

            try
            {
                _subject = new FakeGenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
                result = await _subject.Run(parameters, genValMode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.AreEqual(returnCode, result);
        }
    }
}
