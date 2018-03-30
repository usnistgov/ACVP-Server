using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static int[] VALID_KEYING_OPTIONS = new int[] { 1, 2 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateDirection(parameters, errorResults);
            ValidateKeyingOption(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateDirection(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
        private void ValidateKeyingOption(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.KeyingOption, VALID_KEYING_OPTIONS, "Keying Options");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
