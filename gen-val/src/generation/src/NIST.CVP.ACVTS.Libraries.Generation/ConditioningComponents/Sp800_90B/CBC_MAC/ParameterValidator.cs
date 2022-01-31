using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.CBC_MAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_LENGTHS = { 128, 192, 256 };
        public static int VALID_MIN_PAYLOAD_LENGTH = 128;
        public static int VALID_MAX_PAYLOAD_LENGTH = 65536;
        public static int VALID_INCREMENT = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeyLengths(parameters, errorResults);
            ValidatePayloadLengths(parameters, errorResults);

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
            var valid = ValidateDomain(parameters.PayloadLen, errorResults, "PayloadLen", VALID_MIN_PAYLOAD_LENGTH, VALID_MAX_PAYLOAD_LENGTH);
            if (valid)
            {
                var results = ValidateMultipleOf(parameters.PayloadLen, VALID_INCREMENT, "PayloadLen Increment");
                errorResults.AddIfNotNullOrEmpty(results);
            }
        }
    }
}
