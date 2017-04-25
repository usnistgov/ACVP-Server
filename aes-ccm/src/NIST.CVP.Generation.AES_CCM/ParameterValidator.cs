using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_CCM
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 32, 48, 64, 80, 96, 112, 128 };
        public static int[] VALID_NONCE_LENGTHS = new int[] { 56, 64, 72, 80, 88, 64, 104 };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 256;
        public static int VALID_MIN_AAD = 0;
        public static long VALID_MAX_AAD = long.MaxValue;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateNonce(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateKeySizes(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidatePlainText(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PtLen, "Plain Text Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.PtLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_MIN_PT,
                VALID_MAX_PT,
                "Plain Text Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PtLen, 8, "Plain Text Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateNonce(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.Nonce, "Nonce Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.Nonce.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_NONCE_LENGTHS.Min(),
                VALID_NONCE_LENGTHS.Max(),
                "Nonce Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.Nonce, 8, "Nonce Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateAAD(Parameters parameters,  List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.AadLen, "AAD Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.AadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_MIN_AAD,
                VALID_MAX_AAD,
                "AAD Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.AadLen, 8, "AAD Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);

            if (domain.Maximum >= (1 << 19))
            {
                parameters.SupportsAad2Pow16 = true;
            }
        }

        private void ValidateTagSizes(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.TagLen, "Tag Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.TagLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_TAG_LENGTHS.Min(),
                VALID_TAG_LENGTHS.Max(),
                "Tag Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.TagLen, 16, "Tag Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
