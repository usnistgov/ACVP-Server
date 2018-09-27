using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().Except(new []{ "p-192", "b-163", "k-163" }, StringComparer.OrdinalIgnoreCase).ToArray();
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
