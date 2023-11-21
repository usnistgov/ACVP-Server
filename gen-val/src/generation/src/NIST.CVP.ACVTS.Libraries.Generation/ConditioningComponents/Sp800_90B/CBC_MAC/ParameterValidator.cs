using System.Collections.Generic;
using System.Linq;
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
            ValidateKeys(parameters, errorResults);
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
        
        private void ValidateKeys(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Keys == null) return;
            
            // Validate each key's bit length is supported
            var supportedSizes = new List<int>();
            foreach (var key in parameters.Keys)
            {
                if (!supportedSizes.Contains(key.BitLength))
                {
                    supportedSizes.Add(key.BitLength);
                }
            }

            var result = ValidateArray(supportedSizes, VALID_KEY_LENGTHS, "keys");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            
            // Validate all key's bit lengths are supported in KeyLen
            List<int> keyLenDistinct = parameters.KeyLen.Distinct().ToList();
            foreach (var key in parameters.Keys)
            {
                if (keyLenDistinct.Contains(key.BitLength))
                {
                    keyLenDistinct.Remove(key.BitLength);
                }
            }

            if (keyLenDistinct.Count > 0)
            {
                errorResults.Add($"All custom keys must have a matching supported valid bit length within KeyLen. ");
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
