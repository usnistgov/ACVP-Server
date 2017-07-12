using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_DIRECTIONS = new string[] { "gen", "ver" };
        public static int VALID_MESSAGE_LENGTH_MIN = 0;
        public static int VALID_MESSAGE_LENGTH_MAX = 1 << 19;
        public static int VALID_MAC_LENGTH_MIN = 8;
        public static int VALID_MAC_LENGTH_MAX = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateMessageLength(parameters, errorResults);
            ValidateMacLength(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            var validAlgorithmValues = AlgorithmSpecificationMapping.Map
                .Select(s => s.algoSpecification)
                .ToArray();

            var algoCheck = ValidateValue(parameters.Algorithm, validAlgorithmValues, "Algorithm");
            errorResults.AddIfNotNullOrEmpty(algoCheck);
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

            var modCheck = ValidateMultipleOf(parameters.MacLen, 8, "MacLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
