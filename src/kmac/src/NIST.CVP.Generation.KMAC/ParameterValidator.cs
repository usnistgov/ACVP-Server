using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.KMAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public const int _MIN_KEY_LENGTH = 8;
        public const int _MAX_KEY_LENGTH = 524288;
        public static string[] VALID_ALGORITHMS = {"kmac"};
        public static int[] VALID_DIGEST_SIZES = { 128, 256 };

        private int _minMacLength = 16;
        private int _maxMacLength = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            ValidateFunctions(parameters, errorResults);

            ValidateKeyLen(parameters, errorResults);
            ValidateMacLen(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_ALGORITHMS, "KMAC Function");
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

        private void ValidateKeyLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.KeyLen, "KeyLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.KeyLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _MIN_KEY_LENGTH,
                _MAX_KEY_LENGTH,
                "KeyLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.KeyLen, 8, "KeyLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMacLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.MacLen, "MacLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MacLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _minMacLength,
                _maxMacLength,
                "MacLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }
    }
}
