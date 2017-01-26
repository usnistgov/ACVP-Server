using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int VALID_MAX_MESSAGE = 65536;
        public static int VALID_MIN_MESSAGE = 0;
        public static int VALID_DIGEST = 160;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateMessageSizes(parameters, errorResults);
            ValidateDigestSizes(parameters, errorResults);
            ValidateIncludeNull(parameters, errorResults);
            ValidateBitOriented(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateMessageSizes(Parameters parameters, List<String> errorResults)
        {
            var result = ValidateRange(parameters.MessageLen, VALID_MIN_MESSAGE, VALID_MAX_MESSAGE,
                "Message length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDigestSizes(Parameters parameters, List<String> errorResults)
        {
            var result = ValidateRange(parameters.DigestLen, VALID_DIGEST, VALID_DIGEST, "Digest length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateIncludeNull(Parameters parameters, List<String> errorResults)
        {
            var result = ValidateBoolean(parameters.IncludeNull, "Include Null (true or false)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateBitOriented(Parameters parameters, List<String> errorResults)
        {
            var result = ValidateBoolean(parameters.BitOriented, "Bit oriented (true or false)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
