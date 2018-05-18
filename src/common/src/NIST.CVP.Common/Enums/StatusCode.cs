using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NIST.CVP.Common.Enums
{
    public enum StatusCode
    {
        [EnumMember(Value = "success")]
        Success = 0,

        [EnumMember(Value = "command line error, check arguments and usage")]
        CommandLineError = 1,

        [EnumMember(Value = "general exception, see logs")]
        Exception = 2,

        [EnumMember(Value = "generator error, see logs")]
        GeneratorError = 3,

        [EnumMember(Value = "validator error, see logs")]
        ValidatorError = 4,

        [EnumMember(Value = "invalid mode provided, must run generator or validator and provide needed files")]
        ModeError = 5,

        [EnumMember(Value = "parameter error, unable to parse parameters in registration file")]
        ParameterError = 6,
        
        [EnumMember(Value = "parameter validator error, parameters do not match the required parameters for the algorithm, check appropriate values")]
        ParameterValidationError = 7,

        [EnumMember(Value = "file save error, unable to save json files")]
        FileSaveError = 8,

        [EnumMember(Value = "test case generator error, unable to produce test case for prompt file")]
        TestCaseGeneratorError = 9,

        [EnumMember(Value = "test case validator error, unable to parse provided results file, or expected value is missing")]
        TestCaseValidatorError = 10,

        [EnumMember(Value = "file read error, unable to read internal projection or results file")]
        FileReadError = 11
    }
}
