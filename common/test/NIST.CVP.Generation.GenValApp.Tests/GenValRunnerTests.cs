using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NIST.CVP.Generation.GenValApp.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class GenValRunnerTests
    {
        private GenValRunner _subject;
        private readonly FakeAutofacConfig _fakeAutofac = new FakeAutofacConfig();

        [Test]
        [TestCase(GenValMode.Generate)]
        [TestCase(GenValMode.Validate)]
        public void ShouldCorrectlySetRunningMode(GenValMode genValMode)
        {
            var parameters = new ArgumentParsingTarget
            {
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo("registration.json") : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo("response.json") : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo("answer.json") : null
            };

            _subject = new GenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            _subject.SetRunningMode(parameters);

            Assert.AreEqual(genValMode, _subject.GenValMode);
        }

        [Test]
        [TestCase(GenValMode.Generate)]
        [TestCase(GenValMode.Validate)]
        public void ShouldNotRunWithoutSettingRunningMode(GenValMode genValMode)
        {
            var parameters = new ArgumentParsingTarget
            {
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo("registration.json") : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo("response.json") : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo("answer.json") : null
            };

            _subject = new GenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.Run(parameters);

            Assert.AreNotEqual(0, result);
        }

        [Test]
        public void ShouldRunGeneration()
        {
            _subject = new GenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.RunGeneration("registration.json");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(GenerateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldRunValidation()
        {
            _subject = new GenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.RunValidation("response.json", "answer.json");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ValidateResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(GenValMode.Generate, "registration.json", null, null, 0)]
        [TestCase(GenValMode.Validate, null, "response.json", "answer.json", 0)]
        [TestCase(GenValMode.Unset, null, null, null, 1)]
        public void ShouldRun(GenValMode genValMode, string registrationFile, string responseFile, string answerFile, int returnCode)
        {
            var parameters = new ArgumentParsingTarget
            {
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo(registrationFile) : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo(responseFile) : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo(answerFile) : null
            };

            _subject = new GenValRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            _subject.SetRunningMode(parameters);
            var result = _subject.Run(parameters);

            Assert.AreEqual(returnCode, result);
        }
    }
}
