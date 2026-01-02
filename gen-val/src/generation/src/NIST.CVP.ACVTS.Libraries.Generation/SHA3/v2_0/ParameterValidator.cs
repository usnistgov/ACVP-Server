using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using ParameterValidatorBase = NIST.CVP.ACVTS.Libraries.Generation.Core.ParameterValidatorBase;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" };
        public static int VALID_MIN_MESSAGE_LENGTH = 0;
        public static int VALID_MAX_MESSAGE_LENGTH = 65536;
        public static int[] VALID_LARGE_DATA_SIZES = { 1, 2, 4, 8 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            GetDigestSize(parameters, errorResults);
            ValidateFunctions(parameters, errorResults);
            ValidateLargeDataTest(parameters, errorResults);
            ValidateMessageLength(parameters.MessageLength, errorResults);
            
            return new ParameterValidateResponse(errorResults);
        }
        
        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateValue(parameters.Algorithm, VALID_MODES, "SHA Function");
            errorResults.AddIfNotNullOrEmpty(result);
        }

        private void ValidateLargeDataTest(Parameters parameters, List<string> errorResults)
        {
            // Optional parameter for SHA-3, but cannot be provided for SHAKE
            if (parameters.PerformLargeDataTest != null && parameters.PerformLargeDataTest.Any())
            {
                var result = ValidateArray(parameters.PerformLargeDataTest, VALID_LARGE_DATA_SIZES, "Large data size");
                errorResults.AddIfNotNullOrEmpty(result);
            }
        }
        
        private void ValidateMessageLength(MathDomain messageLength, List<string> errorResults)
        {
            if (messageLength == null)
            {
                errorResults.Add("MessageLength was null and is required.");
                return;
            }
            
            ValidateDomain(messageLength, errorResults, "Message Length", VALID_MIN_MESSAGE_LENGTH, VALID_MAX_MESSAGE_LENGTH);
            
            // Enforce min supported length above MIN_MESSAGE_LENGTH for MCT
            if (!messageLength.GetSequentialValues(VALID_MIN_MESSAGE_LENGTH+1, VALID_MAX_MESSAGE_LENGTH, 2).Any())
            {
                errorResults.Add($"Message length must contain at least one segment that is greater than 0 bits for MCT.");
            }
        }
        
        private void GetDigestSize(Parameters parameters, List<string> errorResults)
        {
            parameters.DigestSizes = new List<string>();
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
    }
}
