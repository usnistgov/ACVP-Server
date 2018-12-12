using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static string[] VALID_KWCIPHER = { "cipher", "inverse" };
        public static int[] VALID_KEYING_OPTIONS = { 1, 2 };
        public static int MINIMUM_PT_LEN = 64;
        public static int MAXIMUM_PT_LEN = 4096;
        public static int PT_MODULUS = 32;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            KeyWrapType keyWrapType = 0;

            ValidateAndGetOptions(parameters, errorResults, ref keyWrapType);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the algorithm.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }
            ValidateDirection(parameters, errorResults);
            ValidateKwCipher(parameters, errorResults);
            ValidatePtLen(parameters, errorResults);
            ValidateKeyingOption(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
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

        private void ValidateKwCipher(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.KwCipher, VALID_KWCIPHER, "KwCipher");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidatePtLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PayloadLen, "PtLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.PayloadLen.GetDomainMinMax();
            if (fullDomain.Minimum < MINIMUM_PT_LEN)
            {
                errorResults.Add($"PtLen minimum must be at least {MINIMUM_PT_LEN}");
            }

            var modCheck = ValidateMultipleOf(parameters.PayloadLen, PT_MODULUS, "PtLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
