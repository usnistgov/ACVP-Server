using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = {"sha3", "shake"};
        public static int[] VALID_DIGEST_SIZES = {128, 224, 256, 384, 512};
        public static int[] VALID_SHA3_DIGEST_SIZES = { 224, 256, 384, 512 };
        public static int[] VALID_SHAKE_DIGEST_SIZES = { 128, 256 };

        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateFunctions(parameters, errorResults);
            ValidateMatching(parameters, errorResults);

            if (parameters.Algorithm.ToLower() == "shake")
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
            if (parameters.Algorithm.ToLower() == "sha3")
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA3_DIGEST_SIZES, "SHA3 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
            else if (parameters.Algorithm.ToLower() == "shake")
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
            string segmentCheck = "";
            if (parameters.OutputLength.DomainSegments.Count() != 1)
            {
                segmentCheck = "Must have exactly one segment in the domain";
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
            var bitOriented = parameters.BitOrientedOutput ? 1 : 8;
            var modCheck = ValidateMultipleOf(parameters.OutputLength, bitOriented, "OutputLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
