using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "p-224", "p-256", "p-384", "p-521", "b-233", "b-283", "b-409", "b-571", "k-233", "k-283", "k-409", "k-571" };
        public static string[] VALID_SECRET_GENERATION_MODES = { "extra bits", "testing candidates" };

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
