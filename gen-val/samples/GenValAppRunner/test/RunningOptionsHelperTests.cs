using System.IO;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Generation.GenValApp.Models;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class RunningOptionsHelperTests
    {
        [Test]
        [TestCase(GenValMode.Check)]
        [TestCase(GenValMode.Generate)]
        [TestCase(GenValMode.Validate)]
        public void ShouldCorrectlySetRunningMode(GenValMode genValMode)
        {
            var parameters = new ArgumentParsingTarget
            {
                CapabilitiesFile = genValMode == GenValMode.Check ? new FileInfo("registration.json") : null,
                RegistrationFile = genValMode == GenValMode.Generate ? new FileInfo("registration.json") : null,
                ResponseFile = genValMode == GenValMode.Validate ? new FileInfo("response.json") : null,
                AnswerFile = genValMode == GenValMode.Validate ? new FileInfo("answer.json") : null
            };

            var result = RunningOptionsHelper.DetermineRunningMode(parameters);

            Assert.That(result, Is.EqualTo(genValMode));
        }
    }
}
