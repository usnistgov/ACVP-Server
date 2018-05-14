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
        ModeError
    }
}
