using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.IO;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class RunningOptionsHelperTests
    {
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

            var result = RunningOptionsHelper.DetermineRunningMode(parameters);

            Assert.AreEqual(genValMode, result);
        }
    }
}