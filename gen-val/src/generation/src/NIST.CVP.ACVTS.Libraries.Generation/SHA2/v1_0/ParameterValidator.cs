using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256" };
        public static string[] VALID_DIGEST_SIZES = { "160", "224", "256", "384", "512", "512/224", "512/256" };
        public static string[] VALID_SHA1_SIZES = { "160" };
        public static string[] VALID_SHA2_SIZES = { "224", "256", "384", "512", "512/224", "512/256" };
        public static int MIN_MESSAGE_LENGTH = 0;
        public static int MAX_MESSAGE_LENGTH = 65536;
        public static int[] VALID_LARGE_DATA_SIZES = { 1, 2, 4, 8 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            // Implementing "default values" to match registration expectations
            if (parameters.DigestSizes == null)
            {
                parameters.DigestSizes = new List<string>();
            }
            if (parameters.DigestSizes.Count == 0)
            {
                var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
                switch (algoMode)
                {
                    case AlgoMode.SHA_1_v1_0:
                        parameters.DigestSizes.Add("160");
                        break;
                    case AlgoMode.SHA2_224_v1_0:
                        parameters.DigestSizes.Add("224");
                        break;
                    case AlgoMode.SHA2_256_v1_0:
                        parameters.DigestSizes.Add("256");
                        break;
                    case AlgoMode.SHA2_384_v1_0:
                        parameters.DigestSizes.Add("384");
                        break;
                    case AlgoMode.SHA2_512_v1_0:
                        parameters.DigestSizes.Add("512");
                        break;
                    case AlgoMode.SHA2_512_224_v1_0:
                        parameters.DigestSizes.Add("512/224");
                        break;
                    case AlgoMode.SHA2_512_256_v1_0:
                        parameters.DigestSizes.Add("512/256");
                        break;

                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }

            ValidateFunctions(parameters, errorResults);
            ValidateMatching(parameters, errorResults);

            if (parameters.PerformLargeDataTest.Any())
            {
                ValidateLargeDataTest(parameters, errorResults);
            }

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
            var result = ValidateValue(parameters.Algorithm, VALID_MODES, "SHA Function");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.DigestSizes, VALID_DIGEST_SIZES, "Digest Size");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidateMatching(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Algorithm.ToLower() == "sha-1")
            {
                var result = ValidateArray(parameters.DigestSizes, VALID_SHA1_SIZES, "SHA1 digest size");
                errorResults.AddIfNotNullOrEmpty(result);
            }
            else if (parameters.Algorithm.ToLower().Contains("sha2"))
            {
                var result = ValidateArray(parameters.DigestSizes, VALID_SHA2_SIZES, "SHA2 digest size");
                errorResults.AddIfNotNullOrEmpty(result);
            }
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var messageLengths = parameters.MessageLength;
            if (errorResults.AddIfNotNullOrEmpty(ValidateSegmentCountGreaterThanZero(messageLengths, "Message Lengths")))
            {
                return;
            }
            
            // Enforce min/max
            var minMax = messageLengths.GetDomainMinMax();
            if (minMax.Minimum < MIN_MESSAGE_LENGTH)
            {
                errorResults.Add($"Message Length Minimum is below allowed value. {minMax.Minimum} < {MIN_MESSAGE_LENGTH}");
            }

            if (minMax.Maximum > MAX_MESSAGE_LENGTH)
            {
              errorResults.Add(
                $"Message Length Maximum is above allowed value. {minMax.Maximum} > {MAX_MESSAGE_LENGTH}");
            }
            
            // Enforce min supported length above MIN_MESSAGE_LENGTH for MCT
            if (!messageLengths.GetValues(MIN_MESSAGE_LENGTH+1, MAX_MESSAGE_LENGTH, 2).Any())
            {
                errorResults.Add($"Message length must contain at least one segment that is greater than 0 bits for MCT.");
            }
        }

        private void ValidateLargeDataTest(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.PerformLargeDataTest, VALID_LARGE_DATA_SIZES, "Large data size");
            errorResults.AddIfNotNullOrEmpty(result);
        }
    }
}
