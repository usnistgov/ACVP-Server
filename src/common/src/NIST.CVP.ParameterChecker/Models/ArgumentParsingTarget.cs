using CommandLineParser.Arguments;
using System.IO;

namespace NIST.CVP.ParameterChecker.Models
{
    public class ArgumentParsingTarget
    {
        [FileArgument('r', "requestFile", FileMustExist = true, Optional = false,
            Description = "The test vector generation registration file")]
        public FileInfo ParameterFile { get; set; }
    }
}