using System.IO;
using CommandLineParser.Arguments;
using CommandLineParser.Validation;

namespace NIST.CVP.ParameterChecker.Models
{
    public class ArgumentParsingTarget
    {
        [ValueArgument(typeof(string), 'a', "algorithm", Optional = false, 
            Description = "The algorithm to in which to generate/validate test vectors")]
        public string Algorithm { get; set; }

        [ValueArgument(typeof(string), 'm', "mode", Optional = true, DefaultValue = "",
            Description = "The algorithm mode in which to generate/validate test vectors (does not apply to all algorithms)")]
        public string Mode { get; set; }

        [ValueArgument(typeof(string), 'R', "revision", Optional = false, DefaultValue = "",
            Description = "The algorithm testing revision.")]
        public string Revision { get; set; }

        [FileArgument('r', "requestFile", FileMustExist = true, Optional = true,
            Description = "The test vector generation registration file")]
        public FileInfo ParameterFile { get; set; }
    }
}