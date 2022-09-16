using CommandLineParser.Arguments;
using CommandLineParser.Validation;
using System.IO;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    [DistinctGroupsCertification("g", "a,b",
        Description = "(g) - used to indicate generation, (a,b) - used to indicate validation")]
    public class ArgumentParsingTarget
    {
        [FileArgument('g', "generationRequestFile", FileMustExist = true, Optional = true,
            Description = "Specify the test vector generation registration file")]
        public FileInfo RegistrationFile { get; set; }

        [FileArgument('a', "answerFile", FileMustExist = true, Optional = true,
            Description = "Specify the answers file to be used to validate a responses file against.")]
        public FileInfo AnswerFile { get; set; }

        [FileArgument('b', "responsesFile", FileMustExist = true, Optional = true,
            Description = "Specify the response file to validate, provided by the IUT.")]
        public FileInfo ResponseFile { get; set; }

        [ValueArgument(typeof(bool), 'e', "showExpected", Optional = true,
            Description = "Show the expected answer property in the validation file")]
        public bool ShowExpected { get; set; }
    }
}
