using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "ed-25519", "ed-448" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            if (parameters.Capabilities.Length == 0)
            {
                errors.Add("No capabilities found");
            }

            foreach (var capability in parameters.Capabilities)
            {
                result = ValidateArray(capability.Curve, VALID_CURVES, "Curves");
                errors.AddIfNotNullOrEmpty(result);
            }

            if ((!parameters.PreHash) && (!parameters.Pure))
            {
                errors.Add("No valid mode chosen");
            }

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
