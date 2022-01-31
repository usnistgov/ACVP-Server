using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEYING_OPTIONS = { 1, 2 };
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static int MINIMUM_DATA_LEN = 1;
        public static int MAXIMUM_DATA_LEN = 64;
        public static int BLOCK_SIZE = 64;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            string result;

            result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.KeyingOption, VALID_KEYING_OPTIONS, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidateDataLength(parameters.PayloadLen, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateDataLength(MathDomain dataLen, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(dataLen, "DataLength Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            if (!dataLen.IsWithinDomain(BLOCK_SIZE))
            {
                errorResults.Add($"DataLength must contain the block size {BLOCK_SIZE}");
            }

            var fullDomain = dataLen.GetDomainMinMax();
            if (fullDomain.Minimum < MINIMUM_DATA_LEN)
            {
                errorResults.Add($"DataLength minimum must be at least {MINIMUM_DATA_LEN}");
            }

            if (fullDomain.Maximum > MAXIMUM_DATA_LEN)
            {
                errorResults.Add($"DataLength maximum must be at most {MAXIMUM_DATA_LEN}");
            }
        }
    }
}
