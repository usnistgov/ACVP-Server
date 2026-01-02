using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHAKE-128", "SHAKE-256" };
        public static int VALID_MIN_MESSAGE_LENGTH = 0;
        public static int VALID_MAX_MESSAGE_LENGTH = 65536;
        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateFunctions(parameters, errorResults);
            
            ValidateDomain(parameters.MessageLength, errorResults, "Message Length", VALID_MIN_MESSAGE_LENGTH, VALID_MAX_MESSAGE_LENGTH);
            ValidateDomain(parameters.OutputLength, errorResults, "Output Length", VALID_MIN_OUTPUT_SIZE, VALID_MAX_OUTPUT_SIZE);
            
            ValidateMessageLength(parameters, errorResults);
            ValidateOutputLength(parameters, errorResults);
            
            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateValue(parameters.Algorithm, VALID_MODES, "SHAKE Function");
            errorResults.AddIfNotNullOrEmpty(result);
        }
        
        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var messageLengths = parameters.MessageLength;
            if (errorResults.AddIfNotNullOrEmpty(ValidateSegmentCountGreaterThanZero(messageLengths, "Message Lengths")))
            {
                return;
            }
            
            // Enforce min/max
            var minMax = messageLengths.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { minMax.Minimum, minMax.Maximum },
                VALID_MIN_MESSAGE_LENGTH,
                VALID_MAX_MESSAGE_LENGTH,
                "MessageLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }
        
        private void ValidateOutputLength(Parameters parameters, List<string> errorResults)
        {
            if (parameters.OutputLength == null)
            {
                errorResults.Add("outputLen was null and is required.");
                return;
            }

            var fullDomain = parameters.OutputLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_OUTPUT_SIZE,
                VALID_MAX_OUTPUT_SIZE,
                "OutputLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }
    }
}
