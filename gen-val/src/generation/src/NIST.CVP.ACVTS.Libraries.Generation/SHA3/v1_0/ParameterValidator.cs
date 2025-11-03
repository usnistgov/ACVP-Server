using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using ParameterValidatorBase = NIST.CVP.ACVTS.Libraries.Generation.Core.ParameterValidatorBase;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512", "SHAKE-128", "SHAKE-256" };
        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        public static int[] VALID_LARGE_DATA_SIZES = { 1, 2, 4, 8 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            GetDigestSize(parameters, errorResults);

            ValidateFunctions(parameters, errorResults);

            if (parameters.Algorithm.Contains("SHA3", StringComparison.OrdinalIgnoreCase) && parameters.PerformLargeDataTest.Any())
            {
                ValidateLargeDataTest(parameters, errorResults);
            }

            if (parameters.Algorithm.Contains("shake", StringComparison.OrdinalIgnoreCase))
            {
                ValidateOutputLength(parameters, errorResults);
            }

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateValue(parameters.Algorithm, VALID_MODES, "SHA3 Function");
            errorResults.AddIfNotNullOrEmpty(result);
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

        private void ValidateLargeDataTest(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.PerformLargeDataTest, VALID_LARGE_DATA_SIZES, "Large data size");
            errorResults.AddIfNotNullOrEmpty(result);
        }
        
        private void GetDigestSize(Parameters parameters, List<string> errorResults)
        {
            parameters.DigestSizes = new List<int>();
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
    }
}
