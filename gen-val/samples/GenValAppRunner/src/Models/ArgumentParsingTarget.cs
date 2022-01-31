using CommandLineParser.Arguments;
using CommandLineParser.Validation;
using System.IO;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    [DistinctGroupsCertification("g", "n,r",
        Description = "(g) - used to indicate generation, (n,r) - used to indicate validation")]
    public class ArgumentParsingTarget
    {
        [FileArgument('g', "generationRequestFile", FileMustExist = true, Optional = true,
            Description = "The test vector generation registration file")]
        public FileInfo RegistrationFile { get; set; }

        [FileArgument('n', "answerFile", FileMustExist = true, Optional = true,
            Description = "The answers file to be used to validate a responses file against.")]
        public FileInfo AnswerFile { get; set; }

        [FileArgument('r', "responsesFile", FileMustExist = true, Optional = true,
            Description = "The response file to validate, provided by the IUT.")]
        public FileInfo ResponseFile { get; set; }

        [ValueArgument(typeof(bool), 'e', "showExpected", Optional = true,
            Description = "Show the expected answer property in the validation file")]
        public bool ShowExpected { get; set; }
    }
}
