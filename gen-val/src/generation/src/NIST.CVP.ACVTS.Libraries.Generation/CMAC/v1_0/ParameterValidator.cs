using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_ALGORITHM = new string[] { "CMAC-AES", "CMAC-TDES" };
        //public static string[] VALID_MODES = new string[] { "AES", "TDES" };
        public static string[] VALID_DIRECTIONS = new string[] { "gen", "ver" };
        public static int[] VALID_KEY_LENGTHS = new int[] { 128, 192, 256 };
        public static int[] VALID_KEYING_OPTIONS = new int[] { 1, 2 };

        public static int VALID_MESSAGE_LENGTH_MIN = 0;
        public static int VALID_MESSAGE_LENGTH_MAX = 1 << 19;
        public static int VALID_MAC_LENGTH_MIN = 1;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
            var mode = ValidateAndGetAlgorithm(parameters.Algorithm, errorResults);
            int maxMacLength = mode == CmacTypes.TDES ? 64 : 128;

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
            {
                return new ParameterValidateResponse(errorResults);
            }

            foreach (var capability in parameters.Capabilities)
            {
                ValidateDirection(capability, errorResults);
                ValidateMessageLength(capability, errorResults);
                ValidateMacLength(capability, maxMacLength, errorResults);

                // AES checks
                ValidateKeyLengths(capability, mode, errorResults);

                // TDES checks
                ValidateKeyingOption(capability, mode, errorResults);
                ValidateKeyingOptionWithMode(capability, mode, errorResults);
            }

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            var algoCheck = ValidateValue(parameters.Algorithm, VALID_ALGORITHM, "Algorithm");
            errorResults.AddIfNotNullOrEmpty(algoCheck);
        }

        private CmacTypes ValidateAndGetAlgorithm(string algorithm, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(algorithm))
            {
                errorResults.AddIfNotNullOrEmpty($"Invalid {nameof(algorithm)} provided {algorithm}");
                return default(CmacTypes);
            }

            if (!AlgorithmSpecificationMapping.Map
                .TryFirst(
                    w => w.algoSpecification.StartsWith(algorithm, StringComparison.OrdinalIgnoreCase),
                    out var result))
            {
                errorResults.AddIfNotNullOrEmpty($"Invalid {nameof(algorithm)} provided {algorithm}");
                return default(CmacTypes);
            }

            return result.mappedCmacType;
        }

        private void ValidateDirection(Capability capability, List<string> errorResults)
        {
            var directionCheck = ValidateArray(capability.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(directionCheck);
        }

        private void ValidateMessageLength(Capability capability, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.MsgLen, "MsgLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = capability.MsgLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MESSAGE_LENGTH_MIN,
                VALID_MESSAGE_LENGTH_MAX,
                "MsgLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(capability.MsgLen, 8, "MsgLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMacLength(Capability capability, int maxMacLength, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.MacLen, "MacLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = capability.MacLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MAC_LENGTH_MIN,
                maxMacLength,
                "MacLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateKeyLengths(Capability capability, CmacTypes mode, List<string> errorResults)
        {
            // Key length only valid for AES
            if (mode == CmacTypes.TDES)
            {
                return;
            }

            var keyLenCheck = ValidateArray(capability.KeyLen, VALID_KEY_LENGTHS, "KeyLen");
            errorResults.AddIfNotNullOrEmpty(keyLenCheck);
        }

        private void ValidateKeyingOption(Capability capability, CmacTypes mode, List<string> errorResults)
        {
            // Keying option only valid for TDES
            if (mode != CmacTypes.TDES)
            {
                return;
            }

            var keyingOptionCheck = ValidateArray(capability.KeyingOption, VALID_KEYING_OPTIONS, "KeyingOption");
            errorResults.AddIfNotNullOrEmpty(keyingOptionCheck);
        }

        private void ValidateKeyingOptionWithMode(Capability capability, CmacTypes mode, List<string> errorResults)
        {
            // Keying option only valid for TDES
            if (mode != CmacTypes.TDES)
            {
                return;
            }

            if (capability.KeyingOption.All(ko => ko == 2) && capability.Direction.All(dir => dir.Equals("gen", StringComparison.OrdinalIgnoreCase)))
            {
                errorResults.AddIfNotNullOrEmpty(@"""gen"" mode is invalid with a keying option of 2");
            }
        }
    }
}
