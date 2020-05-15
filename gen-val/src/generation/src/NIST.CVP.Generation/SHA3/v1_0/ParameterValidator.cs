using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "sha3-224", "sha3-256", "sha3-384", "sha3-512", "shake-128", "shake-256" };
        public static int[] VALID_DIGEST_SIZES = { 128, 224, 256, 384, 512 };
        public static int[] VALID_SHA3_DIGEST_SIZES = { 224, 256, 384, 512 };
        public static int[] VALID_SHAKE_DIGEST_SIZES = { 128, 256 };

        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            // Implementing "default values" to match registration expectations
            if (parameters.DigestSizes == null)
            {
                parameters.DigestSizes = new List<int>();
            }
            if (parameters.DigestSizes.Count == 0)
            {
                var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
                switch (algoMode)
                {
                    case AlgoMode.SHA3_224_v1_0:
                        parameters.DigestSizes.Add(224);
                        break;
                    case AlgoMode.SHA3_256_v1_0:
                        parameters.DigestSizes.Add(256);
                        break;
                    case AlgoMode.SHA3_384_v1_0:
                        parameters.DigestSizes.Add(384);
                        break;
                    case AlgoMode.SHA3_512_v1_0:
                        parameters.DigestSizes.Add(512);
                        break;
                    
                    case AlgoMode.SHAKE_128_v1_0:
                        parameters.DigestSizes.Add(128);
                        break;
                    case AlgoMode.SHAKE_256_v1_0:
                        parameters.DigestSizes.Add(256);
                        break;

                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }
            
            ValidateFunctions(parameters, errorResults);
            ValidateMatching(parameters, errorResults);

            if (parameters.Algorithm.ToLower().Contains("shake"))
            {
                ValidateOutputLength(parameters, errorResults);
            }

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_MODES, "SHA3 Function");
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
            if (parameters.Algorithm.ToLower().Contains("sha3"))
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA3_DIGEST_SIZES, "SHA3 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
            else if (parameters.Algorithm.ToLower().Contains("shake"))
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHAKE_DIGEST_SIZES, "SHAKE digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }

        private void ValidateOutputLength(Parameters parameters, List<string> errorResults)
        {
            if (parameters.OutputLength == null)
            {
                errorResults.Add("outputLen was null and is required.");
                return;
            }
            
            if (parameters.OutputLength.DomainSegments.Count() != 1)
            {
                errorResults.Add("outputLen must have exactly one segment in the domain.");
                return;
            }
            
            var fullDomain = parameters.OutputLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_OUTPUT_SIZE,
                VALID_MAX_OUTPUT_SIZE,
                "OutputLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.BitOrientedOutput ? 1 : 8;
            var modCheck = ValidateMultipleOf(parameters.OutputLength, bitOriented, "OutputLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
