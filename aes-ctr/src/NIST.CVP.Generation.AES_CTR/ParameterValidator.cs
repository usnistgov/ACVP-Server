using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CTR
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static int MINIMUM_DATA_LEN = 1;
        public static int MAXIMUM_DATA_LEN = 128;
        public static int BLOCK_SIZE = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            string result;

            result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidateDataLength(parameters.DataLength, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
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
