using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_LMS_TYPES = EnumHelpers.GetEnumDescriptions<LmsType>().ToArray();
        public static string[] VALID_LMOTS_TYPES = EnumHelpers.GetEnumDescriptions<LmotsType>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            if (parameters.Specific)
            {
                if (parameters.SpecificCapabilities.Length == 0)
                {
                    errors.Add("No capabilities found");
                }

                foreach (var capability in parameters.SpecificCapabilities)
                {
                    foreach (var level in capability.Levels)
                    {
                        result = ValidateValue(level.LmsType, VALID_LMS_TYPES, "Lms Type");
                        errors.AddIfNotNullOrEmpty(result);

                        result = ValidateValue(level.LmotsType, VALID_LMOTS_TYPES, "Lmots Type");
                        errors.AddIfNotNullOrEmpty(result);
                    }
                }
            }
            else
            {
                result = ValidateArray(parameters.Capabilities.LmsTypes, VALID_LMS_TYPES, "Lms Type");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArray(parameters.Capabilities.LmotsTypes, VALID_LMOTS_TYPES, "Lmots Type");
                errors.AddIfNotNullOrEmpty(result);
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
