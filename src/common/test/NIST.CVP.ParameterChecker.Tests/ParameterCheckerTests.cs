using System;
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
        private ParameterCheckRunner _subject;
        private readonly FakeAutofacConfig _fakeAutofac = new FakeAutofacConfig();

        [Test]
        public void ShouldRunOnGoodParameters()
        {
            _subject = new ParameterCheckRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
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
            _subject = new ParameterCheckRunner(_fakeAutofac.GetContainer().BeginLifetimeScope());
            var result = _subject.RunParameterChecker(file);

            Assert.IsNotNull(result, "Not null result");
            Assert.IsInstanceOf(typeof(ParameterCheckResponse), result, "Instance type");
            Assert.IsFalse(result.Success, "Success parsing");
        }
    }
}
