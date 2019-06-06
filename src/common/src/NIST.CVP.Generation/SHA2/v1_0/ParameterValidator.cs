using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.SHA2.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "sha1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };
        public static string[] VALID_DIGEST_SIZES = { "160", "224", "256", "384", "512", "512/224", "512/256" };
        public static string[] VALID_SHA1_SIZES = { "160" };
        public static string[] VALID_SHA2_SIZES = { "224", "256", "384", "512", "512/224", "512/256" };
        public static int MIN_MESSAGE_LENGTH = 0;
        public static int MAX_MESSAGE_LENGTH = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateFunctions(parameters, errorResults);
            ValidateMatching(parameters, errorResults);

            // Duplicated to here to avoid NRE when checking that DigestSizes contains a value
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            ValidateMessageLength(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_MODES, "SHA Function");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.DigestSizes, VALID_DIGEST_SIZES, "Digest Size");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateMatching(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Algorithm.ToLower() == "sha1")
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA1_SIZES, "SHA1 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
            else if (parameters.Algorithm.ToLower() == "sha2")
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA2_SIZES, "SHA2 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var messageLengths = parameters.MessageLength;

            // Enforce min/max
            var minMax = messageLengths.GetDomainMinMax();
            if (minMax.Minimum < MIN_MESSAGE_LENGTH)
            {
                errorResults.Add($"Message Length Minimum is below allowed value. {minMax.Minimum} < {MIN_MESSAGE_LENGTH}");
            }

            if (minMax.Maximum > MAX_MESSAGE_LENGTH)
            {
                errorResults.Add($"Message Length Maximum is above allowed value. {minMax.Maximum} > {MAX_MESSAGE_LENGTH}");
            }

            // Check for digest size within domain
            // Hard assumption that there is 1 value in array (and only 1)
            var digestSize = SHAEnumHelpers.DigestSizeToInt(SHAEnumHelpers.StringToDigest(parameters.DigestSizes.First()));
            if (!messageLengths.IsWithinDomain(digestSize))
            {
                errorResults.Add($"Message Length must contain the digest size itself.");
            }
        }
    }
}
