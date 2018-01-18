using System.IO;
using CommandLineParser.Arguments;
using CommandLineParser.Validation;

namespace NIST.CVP.Generation.GenValApp
{
    [DistinctGroupsCertification("g", "n,p,r", 
        Description = "(g) - used to indicate generation, (n,p,r) - used to indicate validation")]
    public class ArgumentParsingTarget
    {
        [ValueArgument(typeof(Algorithm), 'a', "algorithm", Optional = false, 
            Description = "The algorithm to in which to generate/validate test vectors")]
        public Algorithm Algorithm { get; set; }

        [ValueArgument(typeof(string), 'm', "mode", Optional = true, DefaultValue = "",
            Description = "The algorithm mode in which to generate/validate test vectors (does not apply to all algorithms)")]
        public string Mode { get; set; }

        [DirectoryArgument('d', "dllLocation", DirectoryMustExist = true, Optional = true,
            Description = "The location to find run time loaded DLLs.  When not provided, current working directory is used.")]
        public DirectoryInfo DllLocation { get; set; }

        [FileArgument('g', "generationRequestFile", FileMustExist = true, Optional = true,
            Description = "The test vector generation registration file")]
        public FileInfo RegistrationFile { get; set; }

        [FileArgument('n', "answerFile", FileMustExist = true, Optional = true,
            Description = "The answers file to be used to validate a responses file against.")]
        public FileInfo AnswerFile { get; set; }

        [FileArgument('p', "promptFile", FileMustExist = true, Optional = true,
            Description = "The prompt file to be used to validate a responses file against.")]
        public FileInfo PromptFile { get; set; }

        [FileArgument('r', "responsesFile", FileMustExist = true, Optional = true,
            Description = "The response file to validate, provided by the IUT.")]
        public FileInfo ResponseFile { get; set; }
    }
}