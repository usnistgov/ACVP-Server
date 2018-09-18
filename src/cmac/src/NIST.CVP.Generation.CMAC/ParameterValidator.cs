using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string VALID_ALGORITHM = "CMAC";
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
            var mode = ValidateAndGetMode(parameters.Mode, errorResults);
            int maxMacLength = mode == CmacTypes.TDES ? 64 : 128;

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
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
            
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            var algoCheck = ValidateValue(parameters.Algorithm, new string[] {VALID_ALGORITHM}, "Algorithm");
            errorResults.AddIfNotNullOrEmpty(algoCheck);
        }
        
        private CmacTypes ValidateAndGetMode(string mode, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(mode))
            {
                errorResults.AddIfNotNullOrEmpty($"Invalid {nameof(mode)} provided {mode}");
                return default(CmacTypes);
            }

            if (!AlgorithmSpecificationMapping.Map
                .TryFirst(
                    w => w.algoSpecification.StartsWith(mode, StringComparison.OrdinalIgnoreCase),
                    out var result))
            {
                errorResults.AddIfNotNullOrEmpty($"Invalid {nameof(mode)} provided {mode}");
                return default(CmacTypes);
            }

            return result.mappedCmacType;
        }

        private void ValidateDirection(Capability capability, List<string> errorResults)
        {
            var directionCheck = ValidateArray(new string[] { capability.Direction }, VALID_DIRECTIONS, "Direction");
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
            if (mode == CmacTypes.TDES && capability.KeyLen != 0)
            {
                errorResults.AddIfNotNullOrEmpty("Unexpected keyingLen provided for CMAC TDES");
                return;
            }

            if (mode == CmacTypes.TDES)
            {
                return;
            }

            var keyLenCheck = ValidateArray(new int[] { capability.KeyLen }, VALID_KEY_LENGTHS, "KeyLen");
            errorResults.AddIfNotNullOrEmpty(keyLenCheck);
        }
        
        private void ValidateKeyingOption(Capability capability, CmacTypes mode, List<string> errorResults)
        {
            // Keying option only valid for TDES
            if (mode != CmacTypes.TDES && capability.KeyingOption != 0)
            {
                errorResults.AddIfNotNullOrEmpty("Unexpected keyingOption provided for CMAC AES");
                return;
            }

            if (mode != CmacTypes.TDES)
            {
                return;
            }

            var keyingOptionCheck = ValidateArray(new int[] { capability.KeyingOption }, VALID_KEYING_OPTIONS, "KeyingOption");
            errorResults.AddIfNotNullOrEmpty(keyingOptionCheck);
        }

        private void ValidateKeyingOptionWithMode(Capability capability, CmacTypes mode, List<string> errorResults)
        {
            // Keying option only valid for TDES
            if (mode != CmacTypes.TDES)
            {
                return;
            }

            if (capability.KeyingOption == 2 && capability.Direction == "gen")
            {
                errorResults.AddIfNotNullOrEmpty(@"""gen"" mode is invalid with a keying option of 2");
            }
        }
    }
}
