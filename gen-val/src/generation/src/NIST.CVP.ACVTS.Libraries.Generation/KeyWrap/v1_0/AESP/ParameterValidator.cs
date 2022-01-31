using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AESP
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static string[] VALID_KWCIPHER = { "cipher", "inverse" };
        public static int MINIMUM_PAYLOAD_LEN = 8;
        public static int MAXIMUM_PAYLOAD_LEN = 4096;
        public static int PT_MODULUS = 8;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            KeyWrapType keyWrapType = 0;

            ValidateAndGetOptions(parameters, errorResults, ref keyWrapType);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the algorithm.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            ValidateDirection(parameters, errorResults);
            ValidateKwCipher(parameters, errorResults);
            ValidateKeySize(parameters, errorResults);
            ValidatePayloadLen(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateAndGetOptions(Parameters parameters, List<string> errorResults, ref KeyWrapType keyWrapType)
        {
            if (SpecificationToDomainMapping.Map
                .TryFirst(w =>
                    w.algorithm == parameters.Algorithm, out var result))
            {
                keyWrapType = result.keyWrapType;
            }
            else
            {
                errorResults.Add("Invalid Algorithm provided.");
            }
        }

        private void ValidateDirection(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidateKwCipher(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.KwCipher, VALID_KWCIPHER, "KwCipher");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidateKeySize(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidatePayloadLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PayloadLen, $"{nameof(parameters.PayloadLen)} Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.PayloadLen.GetDomainMinMax();
            if (fullDomain.Minimum < MINIMUM_PAYLOAD_LEN)
            {
                errorResults.Add($"{nameof(parameters.PayloadLen)} minimum must be at least {MINIMUM_PAYLOAD_LEN}");
            }

            var modCheck = ValidateMultipleOf(parameters.PayloadLen, PT_MODULUS, $"{nameof(parameters.PayloadLen)} Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
