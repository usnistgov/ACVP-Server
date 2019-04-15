using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = {128, 256};
        public static string[] VALID_DIRECTIONS = {"encrypt", "decrypt"};
        public static string[] VALID_TWEAKS = {"hex", "number"};
        public static int MINIMUM_PT_LEN = 128;
        public static int MAXIMUM_PT_LEN = 65536;
        public static int PT_MODULUS = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            string result;

            result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.TweakModes, VALID_TWEAKS, "Tweaks");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidatePtLen(parameters.PayloadLen, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidatePtLen(MathDomain ptLen, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(ptLen, "PtLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = ptLen.GetDomainMinMax();
            if (fullDomain.Minimum < MINIMUM_PT_LEN)
            {
                errorResults.Add($"PtLen minimum must be at least {MINIMUM_PT_LEN}");
            }

            if (fullDomain.Maximum > MAXIMUM_PT_LEN)
            {
                errorResults.Add($"PtLen maximum must be at most {MAXIMUM_PT_LEN}");
            }
        }
    }
}
