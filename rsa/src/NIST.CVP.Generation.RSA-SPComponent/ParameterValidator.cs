using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_KEY_FORMATS = EnumHelpers.GetEnumDescriptions<PrivateKeyModes>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            var result = ValidateValue(parameters.KeyFormat, VALID_KEY_FORMATS, "Private key format");
            errors.AddIfNotNullOrEmpty(result);

            if (errors.Count != 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
