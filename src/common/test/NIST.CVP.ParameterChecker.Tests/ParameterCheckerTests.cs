using System;
using System.IO;
using Autofac;
using Moq;
using NIST.CVP.ParameterChecker.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core;
using NIST.CVP.ParameterChecker.Helpers;

namespace NIST.CVP.ParameterChecker.Tests
{
    [TestFixture, UnitTest]
    public class ParameterCheckerTests
    {
        private class FakeParameterCheckRunner : ParameterCheckRunner
        {
            public FakeParameterCheckRunner(IComponentContext scope, IFileService fileService) : base(scope)
            {
                FileService = fileService;
            }
        }
        
        private ParameterCheckRunner _subject;
        private readonly FakeAutofacConfig _fakeAutofac = new FakeAutofacConfig();

        [Test]
        public void ShouldRunOnGoodParameters()
        {
            var fileService = new Mock<IFileService>();
            fileService.Setup(s => s.ReadFile(It.IsAny<string>())).Returns(string.Empty);
            
            _subject = new FakeParameterCheckRunner(_fakeAutofac.GetContainer().BeginLifetimeScope(), fileService.Object);
            var result = _subject.RunParameterChecker("registration.json");

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ParameterCheckResponse), result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("notJson.json")]
        [TestCase("badRegistration.json")]
        public void ShouldRunOnBadParameters(string file)
        {
            var fileService = new Mock<IFileService>();
            fileService.Setup(s => s.ReadFile(It.IsAny<string>())).Throws(new FileNotFoundException());
            _subject = new FakeParameterCheckRunner(_fakeAutofac.GetContainer().BeginLifetimeScope(), fileService.Object);
            var result = _subject.RunParameterChecker(file);

            Assert.IsNotNull(result, "Not null result");
            Assert.IsInstanceOf(typeof(ParameterCheckResponse), result, "Instance type");
            Assert.IsFalse(result.Success, "Success parsing");
        }
    }
}
