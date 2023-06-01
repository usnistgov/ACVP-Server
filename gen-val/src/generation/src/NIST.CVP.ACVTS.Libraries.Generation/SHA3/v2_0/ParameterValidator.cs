using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" };
        public static string[] VALID_DIGEST_SIZES = { "160", "224", "256", "384", "512" };
        public static int VALID_MIN_MESSAGE_LENGTH = 0;
        public static int VALID_MAX_MESSAGE_LENGTH = 65536;
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
                    case AlgoMode.SHA3_224_v2_0:
                        parameters.DigestSizes.Add("224");
                        break;
                    case AlgoMode.SHA3_256_v2_0:
                        parameters.DigestSizes.Add("256");
                        break;
                    case AlgoMode.SHA3_384_v2_0:
                        parameters.DigestSizes.Add("384");
                        break;
                    case AlgoMode.SHA3_512_v2_0:
                        parameters.DigestSizes.Add("512");
                        break;
                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }

            ValidateFunctions(parameters, errorResults);
            ValidateDomain(parameters.MessageLength, errorResults, "Message Length", VALID_MIN_MESSAGE_LENGTH, VALID_MAX_MESSAGE_LENGTH);

            if (parameters.PerformLargeDataTest.Any())
            {
                ValidateLargeDataTest(parameters, errorResults);
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
        
        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var messageLengths = parameters.MessageLength;
            if (errorResults.AddIfNotNullOrEmpty(ValidateSegmentCountGreaterThanZero(messageLengths, "Message Lengths")))
            {
                return;
            }
            
            // Enforce min/max
            var minMax = messageLengths.GetDomainMinMax();
            if (minMax.Minimum < VALID_MIN_MESSAGE_LENGTH)
            {
                errorResults.Add($"Message Length Minimum is below allowed value. {minMax.Minimum} < {VALID_MIN_MESSAGE_LENGTH}");
            }

            if (minMax.Maximum > VALID_MAX_MESSAGE_LENGTH)
            {
                errorResults.Add(
                    $"Message Length Maximum is above allowed value. {minMax.Maximum} > {VALID_MAX_MESSAGE_LENGTH}");
            }
            
            // Enforce min supported length above MIN_MESSAGE_LENGTH for MCT
            if (!messageLengths.GetValues(VALID_MIN_MESSAGE_LENGTH+1, VALID_MAX_MESSAGE_LENGTH, 2).Any())
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
