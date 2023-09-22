using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static int MINIMUM_DATA_LEN = 1;
        public static int MAXIMUM_DATA_LEN = 128;
        public static int BLOCK_SIZE = 128;
        public static string[] ValidConformances = new[] { "RFC3686" };
        public static IvGenModes[] ValidIvGenModes = EnumHelpers.GetEnumsWithoutDefault<IvGenModes>().ToArray();
        public bool _isRfcTesting { get; set; }

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            string result;

            result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidateDataLength(parameters.PayloadLen, errorResults);

            ValidateConformances(parameters, errorResults);

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

        private void ValidateConformances(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Conformances != null && parameters.Conformances.Length != 0)
            {
                errorResults.AddIfNotNullOrEmpty(
                    ValidateArray(parameters.Conformances, ValidConformances, "Conformances"));

                errorResults.AddIfNotNullOrEmpty(
                    ValidateArray(new[] { parameters.IvGenMode }, ValidIvGenModes, "IV Generation Modes"));

                if (!parameters.IncrementalCounter)
                {
                    errorResults.Add("RFC3686 requires the use of an incremental counter.");
                }
            }
            else
            {
                if (parameters.IvGenMode != IvGenModes.None)
                {
                    errorResults.Add("IV Generation Modes are not valid except when running under RFC3686");
                }
            }
        }
    }
}
