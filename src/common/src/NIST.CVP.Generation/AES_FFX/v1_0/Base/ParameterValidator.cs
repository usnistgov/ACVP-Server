using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{


    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?

        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static int VALID_MIN_TWEAK = 0;
        public static int VALID_MAX_TWEAK = 128;
        public static int VALID_MIN_RADIX = 2;
        public static int VALID_MAX_RADIX = 62;
        private AlgoMode _algoMode;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            _algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateCapabilities(parameters, errorResults);
            ValidateTweakLen(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
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

        private void ValidateCapabilities(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Capabilities == null || parameters.Capabilities?.Length == 0)
            {
                errorResults.Add("Must provide at least one set of Capabilities. ");
                return;
            }

            foreach (var capability in parameters.Capabilities)
            {
                if (capability.Radix < VALID_MIN_RADIX || capability.Radix > VALID_MAX_RADIX)
                {
                    errorResults.Add("Radix must be at least 2 and at most 62.");
                }

                if (!NumeralString.IsAlphabetValid(capability.Alphabet))
                {
                    errorResults.Add($"Provided {nameof(capability.Alphabet)} is invalid. Ensure alphabet is made up of between 2 and 2^16 unique characters. ");
                }

                IsRadixValidWithPayload(capability.Radix, capability.MinLen, capability.MaxLen, errorResults);
            }
        }

        private void ValidateTweakLen(Parameters parameters, List<string> errorResults)
        {
            // Static tweaklen for FF3_1
            if (_algoMode == AlgoMode.AES_FF3_1_v1_0)
            {
                parameters.TweakLen = new MathDomain().AddSegment(new ValueDomainSegment(56));
                return;
            }

            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.TweakLen, "TweakLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.TweakLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_TWEAK,
                VALID_MAX_TWEAK,
                "TweakLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.TweakLen, 8, "TweakLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private bool IsRadixValidWithPayload(int radix, int minPayload, int maxPayload, List<string> errorResults)
        {
            const int maxFfxPayloadLength = 1000000;

            // radix ∈ [2..Pow(2, 16) ],
            if (radix < 2 || radix > (int)System.Math.Pow(2, 16))
            {
                errorResults.Add($"{nameof(radix)} not within range of 2...Pow(2, 16). ");
                return false;
            }

            // Pow(radix, minlen) ≥ 1 000 000, and
            if (System.Math.Pow(radix, minPayload) < maxFfxPayloadLength)
            {
                errorResults.Add($"Pow({nameof(radix)}, {nameof(minPayload)}) (Pow({radix}, {minPayload})) must be greater than {maxFfxPayloadLength}. ");
                return false;
            }

            // 2 ≤ minlen ≤ maxlen ≤ 2*Floor(logradix (Pow(2, 96) )).
            if (minPayload < 2)
            {
                errorResults.Add($"{nameof(minPayload)} must be greater than or equal to 2. ");
            }

            if (maxPayload < minPayload)
            {
                errorResults.Add($"{nameof(maxPayload)} cannot be less than {nameof(minPayload)}");
            }

            var maxForRadix = _algoMode == AlgoMode.AES_FF3_1_v1_0 ?
                2 * (int)System.Math.Floor(System.Math.Log(System.Math.Pow(2, 96), radix)) :
                System.Math.Pow(2, 32);
            if (maxPayload <=
                maxForRadix)
            {
                return true;
            }

            errorResults.Add($"The max allowed length for {nameof(radix)} {radix} is {maxForRadix}. ");
            return false;
        }
    }
}
