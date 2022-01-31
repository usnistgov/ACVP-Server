using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Common.Enums
{
    public enum StatusCode
    {
        [EnumMember(Value = "success")]
        Success = 0,

        [EnumMember(Value = "Command line error, check arguments and usage. Contact service provider.")]
        CommandLineError = 1,

        [EnumMember(Value = "General exception. Contact service provider.")]
        Exception = 2,

        [EnumMember(Value = "Generator error. Contact service provider.")]
        GeneratorError = 3,

        [EnumMember(Value = "Validator error. Contact service provider.")]
        ValidatorError = 4,

        [EnumMember(Value = "Invalid mode provided. Must run generator or validator and provide needed files.")]
        ModeError = 5,

        [EnumMember(Value = "Parameter error. Unable to parse parameters in registration file.")]
        ParameterError = 6,

        [EnumMember(Value = "Parameter validator error. Parameters do not match the required parameters for the algorithm, check the appropriate values.")]
        ParameterValidationError = 7,

        [EnumMember(Value = "File save error. Unable to save json files. Contact service provider.")]
        FileSaveError = 8,

        [EnumMember(Value = "Test case generator error. Unable to produce test case for prompt file. Contact service provider.")]
        TestCaseGeneratorError = 9,

        [EnumMember(Value = "Test case validator error. Unable to parse provided results file, or expected value is missing.")]
        TestCaseValidatorError = 10,

        [EnumMember(Value = "File read error, unable to read internal projection or results file. Contact service provider.")]
        FileReadError = 11,

        [EnumMember(Value = "BitString parse error, unable to read one or more BitStrings.  Contact service provider.")]
        BitStringParseError = 12,

        [EnumMember(Value = "Validation failed. Incorrect responses provided.")]
        ValidatorFail = 13
    }
}
