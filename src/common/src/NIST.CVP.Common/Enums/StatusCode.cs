using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NIST.CVP.Common.Enums
{
    public enum StatusCode
    {
        [EnumMember(Value = "success")]
        Success,

        [EnumMember(Value = "command line error, check arguments and usage")]
        CommandLineError,

        [EnumMember(Value = "general exception, see logs")]
        Exception,

        [EnumMember(Value = "generator error, see logs")]
        GeneratorError,

        [EnumMember(Value = "validator error, see logs")]
        ValidatorError,

        [EnumMember(Value = "invalid mode provided, must run generator or validator and provide needed files")]
        ModeError,

        [EnumMember(Value = "parameter error, unable to parse parameters in registration file")]
        ParameterError,
        
        [EnumMember(Value = "parameter validator error, parameters do not match the required parameters for the algorithm, check appropriate values")]
        ParameterValidationError,

        [EnumMember(Value = "file save error, unable to save json files")]
        FileSaveError,

        [EnumMember(Value = "test case generator error, unable to produce test case for prompt file")]
        TestCaseGeneratorError,

        [EnumMember(Value = "test case validator error, unable to parse provided results file, or expected value is missing")]
        TestCaseValidatorError,

        [EnumMember(Value = "file read error, unable to read internal projection or results file")]
        FileReadError
    }
}
