using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "p-192", "p-224", "p-256", "p-384", "p-521", "b-163", "b-233", "b-283", "b-409", "b-571", "k-163", "k-233", "k-283", "k-409", "k-571" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            result = ValidateArray(parameters.Curve, VALID_CURVES, "Curves");
            errors.AddIfNotNullOrEmpty(result);

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
