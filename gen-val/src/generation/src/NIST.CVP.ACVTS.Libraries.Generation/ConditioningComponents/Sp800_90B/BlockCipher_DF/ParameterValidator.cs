using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.BlockCipher_DF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_INPUT_LEN = 8;
        public static int MAX_INPUT_LEN = 65536;
        public static int VALID_INCREMENT = 8;
        public static int[] VALID_KEY_LENGTHS = { 128, 192, 256 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidatePayloadLengths(parameters, errorResults);
            ValidateKeyLengths(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateKeyLengths(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_LENGTHS, "KeyLen");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidatePayloadLengths(Parameters parameters, List<string> errorResults)
        {
            var valid = ValidateDomain(parameters.PayloadLen, errorResults, "PayloadLen", MIN_INPUT_LEN, MAX_INPUT_LEN);
            if (valid)
            {
                var results = ValidateMultipleOf(parameters.PayloadLen, VALID_INCREMENT, "PayloadLen Increment");
                errorResults.AddIfNotNullOrEmpty(results);
            }
        }
    }
}
