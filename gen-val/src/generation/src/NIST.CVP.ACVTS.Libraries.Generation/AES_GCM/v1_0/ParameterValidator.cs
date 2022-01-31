using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_GCM.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?

        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 32, 64, 96, 104, 112, 120, 128 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static string[] VALID_IV_GEN = new string[] { "internal", "external" };
        public static string[] VALID_IV_GEN_MODE = new string[] { "8.2.1", "8.2.2" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 65536;
        public static int VALID_MIN_IV = 8;
        public static int VALID_MAX_IV = 1024;
        private AlgoMode _algoMode;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            _algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateIV(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidatePlainText(Parameters parameters, List<string> errorResults)
        {
            // GMAC is equivalent to GCM, but always uses a 0 length payload.
            if (_algoMode == AlgoMode.AES_GMAC_v1_0)
            {
                parameters.PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(0));
                return;
            }

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

        private void ValidateTagSizes(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.TagLen, VALID_TAG_LENGTHS, "tagLen"));
        }

        private void ValidateAAD(Parameters parameters, List<string> errorResults)
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

        private void ValidateIV(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.IvLen, "ivLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.IvLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_IV,
                VALID_MAX_IV,
                "ivLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.IvLen, 8, "ivLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);


            var result = ValidateValue(parameters.IvGen, VALID_IV_GEN, "IV Generation");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            // Only validate ivGenMode when ivGen is not null and is internal
            if (!string.IsNullOrEmpty(parameters.IvGen) && parameters.IvGen.Equals("internal", StringComparison.CurrentCultureIgnoreCase))
            {
                result = ValidateValue(parameters.IvGenMode, VALID_IV_GEN_MODE, "IV Generation Mode (Internal)");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }
    }
}
