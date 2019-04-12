using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.EDDSA.v1_0.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "ed-25519", "ed-448" };
        public static string[] VALID_SECRET_GENERATION_MODES = EnumHelpers.GetEnumDescriptions<SecretGenerationMode>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            result = ValidateArray(parameters.Curve, VALID_CURVES, "Curves");
            errors.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.SecretGenerationMode, VALID_SECRET_GENERATION_MODES, "Secret Generation Modes");
            errors.AddIfNotNullOrEmpty(result);

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
