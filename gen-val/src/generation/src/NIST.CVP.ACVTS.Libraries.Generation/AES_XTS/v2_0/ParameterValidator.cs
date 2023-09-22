using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_KEY_SIZES = { 128, 256 };        // 192 is not allowed
        public static int MINIMUM_PT_LEN = 128;
        public static int MAXIMUM_PT_LEN = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            string result;

            if (parameters.Direction.Length == 0 || parameters.Direction.Length > 2)
            {
                errorResults.Add($"{nameof(parameters.Direction)} must contain 1 or 2 values");
            }

            if (parameters.TweakMode.Length == 0 || parameters.TweakMode.Length > 2)
            {
                errorResults.Add($"{nameof(parameters.TweakMode)} must contain 1 or 2 values");
            }

            result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidateDomain(parameters.PayloadLen, errorResults, nameof(parameters.PayloadLen));

            if (!parameters.DataUnitLenMatchesPayload)
            {
                ValidateDomain(parameters.DataUnitLen, errorResults, nameof(parameters.DataUnitLen));
                
                // Avoid improper values going into the next step of logic by ejecting on this error
                if (errorResults.Count != 0)
                {
                    return new ParameterValidateResponse(errorResults);
                }
                
                // Need to check that a valid DataUnitLen and PayloadLen pair exist for the multi-data-unit tests
                if (!CheckValidTestCases(parameters.PayloadLen.GetDeepCopy(), parameters.DataUnitLen.GetDeepCopy()))
                {
                    errorResults.Add($"Unable to build test cases where multiple data units fit within one payload; this may be because the {nameof(parameters.DataUnitLen)} and {nameof(parameters.PayloadLen)} are overly restrictive");   
                }
            }
            else
            {
                if (parameters.DataUnitLen != null)
                {
                    errorResults.Add($"{nameof(parameters.DataUnitLen)} is not used when {nameof(parameters.DataUnitLenMatchesPayload)} is true and must be excluded");
                }
            }

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateDomain(MathDomain domain, List<string> errorResults, string friendlyName)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(domain, friendlyName);
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = domain.GetDomainMinMax();
            if (fullDomain.Minimum < MINIMUM_PT_LEN)
            {
                errorResults.Add($"{friendlyName} minimum must be at least {MINIMUM_PT_LEN}");
            }

            if (fullDomain.Maximum > MAXIMUM_PT_LEN)
            {
                errorResults.Add($"{friendlyName} maximum must be at most {MAXIMUM_PT_LEN}");
            }
        }

        private bool CheckValidTestCases(MathDomain payloadLen, MathDomain dataUnitLen)
        {
            var payloadLengths = payloadLen.GetSequentialValues(_ => true, 65536).Distinct();
            
            // We need a dataUnitLength that provides (p % du) >= 128 so that the last data unit has at least one block of content. This is a gap in the XTS standard.
            return payloadLengths.Any(p => dataUnitLen.GetSequentialValues(du => (du <= p) && (p % du >= 128), 1).Any());
        }
    }
}
