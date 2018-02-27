using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 32, 64, 96, 104, 112, 120, 128 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static string[] VALID_IV_GEN = new string[] { "internal", "external" };
        public static string[] VALID_IV_GEN_MODE = new string[] { "8.2.1", "8.2.2" };
        public static string[] VALID_SALT_GEN = new string[] { "internal", "external" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateIV(parameters, errorResults);
            ValidateSalt(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidatePlainText(Parameters parameters,  List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PtLen, "PtLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.PtLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_PT,
                VALID_MAX_PT,
                "PtLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PtLen, 8, "PtLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
        
        private void ValidateKeySizes(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDirection(Parameters parameters,  List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateTagSizes(Parameters parameters,  List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.TagLen, "TagLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            // specific values valid for tag
            bool validTagFound = false;
            foreach (var validTagLength in VALID_TAG_LENGTHS)
            {
                if (parameters.TagLen.IsWithinDomain(validTagLength))
                {
                    validTagFound = true;
                    break;
                }
            }

            if (!validTagFound)
            {
                errorResults.AddIfNotNullOrEmpty("No valid tagLengths provided.");
            }

            var modCheck = ValidateMultipleOf(parameters.TagLen, 8, "TagLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateAAD(Parameters parameters,  List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.aadLen, "AadLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.aadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_AAD,
                VALID_MAX_AAD,
                "AadLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.aadLen, 8, "AadLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateIV(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateValue(parameters.ivGen, VALID_IV_GEN, "IV Generation");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            // Only validate ivGenMode when ivGen is not null and is internal
            if (!string.IsNullOrEmpty(parameters.ivGen) && parameters.ivGen.Equals("internal", StringComparison.CurrentCultureIgnoreCase))
            {
                result = ValidateValue(parameters.ivGenMode, VALID_IV_GEN_MODE, "IV Generation Mode (Internal)");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }

        private void ValidateSalt(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateValue(parameters.SaltGen, VALID_SALT_GEN, "Salt Generation");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
