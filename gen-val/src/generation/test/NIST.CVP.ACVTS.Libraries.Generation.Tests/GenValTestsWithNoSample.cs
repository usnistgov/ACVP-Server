using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests
{
    /// <summary>
    /// Used to run generation against a not sample parameters, in addition to the normal integration tests.
    ///
    /// Ideally the abstract methods here, and in the base would return TParameters,
    /// rather than the file location of the saved parameters file.
    ///
    /// Since that will be more work, for the moment, introduce new abstract that will
    /// allow for the specifying of a parameters with a not sample flag, this is not currently
    /// validated against, so be wary.
    /// </summary>
    public abstract class GenValTestsWithNoSample : GenValTestsSingleRunnerBase
    {
        [Test]
        public async Task ShouldSuccessfullyGenerateWhenNotSample()
        {
            var targetFolder = GetTestFolder("NotSample");
            var fileName = GetTestFileFewTestCasesNotSample(targetFolder);

            LoggingHelper.ConfigureLogging(fileName, "generator", LogLevel.Debug);
            GenLogger.Info($"{Algorithm}-{Mode} Test Vectors");
            var result = await RunGeneration(targetFolder, fileName, true);

            Assert.That(result.Success, Is.True);
        }

        protected abstract string GetTestFileFewTestCasesNotSample(string folderName);
    }
}
