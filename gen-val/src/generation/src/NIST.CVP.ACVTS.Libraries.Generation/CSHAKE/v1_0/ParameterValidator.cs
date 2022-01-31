using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.CSHAKE.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_ALGORITHMS = { "CSHAKE-128", "CSHAKE-256" };
        public static int[] VALID_DIGEST_SIZES = { 128, 256 };

        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        private int _minMsgLength = 0;
        private int _maxMsgLength = 65536;

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
                    case AlgoMode.CSHAKE_128_v1_0:
                        parameters.DigestSizes.Add(128);
                        break;
                    case AlgoMode.CSHAKE_256_v1_0:
                        parameters.DigestSizes.Add(256);
                        break;

                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }

            ValidateFunctions(parameters, errorResults);
            ValidateOutputLength(parameters, errorResults);
            ValidateMessageLength(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm, VALID_ALGORITHMS, "cSHAKE Function");
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

        private void ValidateOutputLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.OutputLength, "OutputLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            if (parameters.OutputLength.DomainSegments.Count() != 1)
            {
                segmentCheck = "OutputLength must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
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
            var bitOriented = parameters.OutputLength.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.OutputLength, bitOriented, "OutputLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.MessageLength, "MessageLength Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            if (parameters.MessageLength.DomainSegments.Count() != 1)
            {
                segmentCheck = "MessageLength must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MessageLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _minMsgLength,
                _maxMsgLength,
                "MessageLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.MessageLength.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.MessageLength, bitOriented, "MessageLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
