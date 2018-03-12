using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string VALID_ALGORITHM = "CMAC";
        public static string VALID_MODE = "AES";
        public static string[] VALID_DIRECTIONS = new string[] { "gen", "ver" };
        public static int[] VALID_KEY_LENGTHS = new int[] {128, 192, 256};
        public static int VALID_MESSAGE_LENGTH_MIN = 0;
        public static int VALID_MESSAGE_LENGTH_MAX = 1 << 19;
        public static int VALID_MAC_LENGTH_MIN = 1;
        public static int VALID_MAC_LENGTH_MAX = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            ValidateDirection(parameters, errorResults);
            ValidateMessageLength(parameters, errorResults);
            ValidateMacLength(parameters, errorResults);
            ValidateKeyLens(parameters, errorResults);

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

            var modeCheck = ValidateValue(parameters.Mode, new string[] { VALID_MODE }, "Mode");
            errorResults.AddIfNotNullOrEmpty(modeCheck);
        }

        private void ValidateDirection(Parameters parameters, List<string> errorResults)
        {
            var directionCheck = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            errorResults.AddIfNotNullOrEmpty(directionCheck);
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.MsgLen, "MsgLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MsgLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MESSAGE_LENGTH_MIN,
                VALID_MESSAGE_LENGTH_MAX,
                "MsgLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.MsgLen, 8, "MsgLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMacLength(Parameters parameters, List<string> errorResults)
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
                VALID_MAC_LENGTH_MIN,
                VALID_MAC_LENGTH_MAX,
                "MacLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateKeyLens(Parameters parameters, List<string> errorResults)
        {
            var keyLenCheck = ValidateArray(parameters.KeyLen, VALID_KEY_LENGTHS, "KeyLen");
            errorResults.AddIfNotNullOrEmpty(keyLenCheck);
        }
    }
}
