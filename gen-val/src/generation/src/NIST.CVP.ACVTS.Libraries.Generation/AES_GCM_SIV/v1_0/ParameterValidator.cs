using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM_SIV.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = new int[] { 128, 256 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidatePlaintextSizes(parameters, errorResults);
            ValidateAADSizes(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidatePlaintextSizes(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PayloadLen, "PtLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.PayloadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_PT,
                VALID_MAX_PT,
                "PtLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PayloadLen, 8, "PtLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateKeySizes(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
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

        private void ValidateAADSizes(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.AadLen, "AadLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.AadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_AAD,
                VALID_MAX_AAD,
                "AadLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.AadLen, 8, "AadLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
