namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests
{
    // [TestFixture, UnitTest]
    // public class ArgumentParsingHelperTests
    // {
    //     private ArgumentParsingHelper _subject;
    //     private string _directory;
    //
    //     [SetUp]
    //     public void SetUp()
    //     {
    //         _subject = new ArgumentParsingHelper();
    //         _directory = Utilities.GetConsistentTestingStartPath(GetType(), "../../testFiles");
    //     }
    //
    //     [Test]
    //     public void ShouldParseGeneratorArgumentsCorrectly()
    //     {
    //         var registration = Path.Combine(_directory, "registration.json");
    //         var args = new[] { "-g", registration };
    //         var result = _subject.Parse(args);
    //
    //         Assert.AreEqual(registration, result.RegistrationFile.ToString(), nameof(result.RegistrationFile));
    //     }
    //
    //     [Test]
    //     public void ShouldParseValidatorArgumentsCorrectly()
    //     {
    //         var answer = Path.Combine(_directory, "answer.json");
    //         var response = Path.Combine(_directory, "response.json");
    //         var args = new[] { "-n", answer, "-r", response };
    //         var result = _subject.Parse(args);
    //
    //         Assert.AreEqual(answer, result.AnswerFile.ToString(), nameof(result.AnswerFile));
    //         Assert.AreEqual(response, result.ResponseFile.ToString(), nameof(result.ResponseFile));
    //     }
    //
    //     [Test]
    //     public void ShouldNotParseIncorrectArguments()
    //     {
    //         var registration = Path.Combine(_directory, "registration.json");
    //         var answer = Path.Combine(_directory, "answer.json");
    //         var response = Path.Combine(_directory, "response.json");
    //
    //         var args = new[] { "-g", registration, "-n", answer, "-r", response };
    //         Assert.Throws<ArgumentConflictException>(() => _subject.Parse(args));
    //     }
    // }
}
