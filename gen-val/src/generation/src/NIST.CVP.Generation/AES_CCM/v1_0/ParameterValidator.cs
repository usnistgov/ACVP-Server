using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = {128, 192, 256};
        public static int[] VALID_TAG_LENGTHS = {32, 48, 64, 80, 96, 112, 128};
        public static int[] VALID_NONCE_LENGTHS = {56, 64, 72, 80, 88, 64, 104};
        public static string[] VALID_CONFORMANCES = {"802.11", "ECMA"};
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 256;
        public static int VALID_MAX_PT_ECMA = 4095 * 8;
        public static int VALID_MIN_AAD = 0;
        public static long VALID_MAX_AAD = long.MaxValue;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateConformances(parameters, errorResults);
            ValidateKeySizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateNonce(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateConformances(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Conformances.Length != 0)
            {
                var result = ValidateArray(parameters.Conformances, VALID_CONFORMANCES, "Conformances");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }

            if (parameters.Conformances.Contains("ecma", StringComparer.OrdinalIgnoreCase))
            {
                if (!parameters.TagLen.IsWithinDomain(8 * 8))
                {
                    errorResults.Add("ECMA must support tagLen of 64 bits");
                }

                if (!parameters.IvLen.IsWithinDomain(13 * 8))
                {
                    errorResults.Add("ECMA must support nonceLen of 104 bits");
                }
            }
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
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PayloadLen, "Plain Text Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.PayloadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_MIN_PT,
                parameters.Conformances.Contains("ecma", StringComparer.OrdinalIgnoreCase) ? VALID_MAX_PT_ECMA : VALID_MAX_PT,
                "Plain Text Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PayloadLen, 8, "Plain Text Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateNonce(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.IvLen, "Nonce Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var domain = parameters.IvLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { domain.Minimum, domain.Maximum },
                VALID_NONCE_LENGTHS.Min(),
                VALID_NONCE_LENGTHS.Max(),
                "Nonce Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.IvLen, 8, "Nonce Modulus");
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
