using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = new string[] {"sha1", "sha2"};

        public static string[] VALID_DIGEST_SIZES = new string[]
            {"160", "224", "256", "384", "512", "512t224", "512t256"};

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            ValidateModes(parameters, errorResults);
            ValidateDigestSizes(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateModes(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.Mode, VALID_MODES, "SHA Function");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDigestSizes(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.DigestSize, VALID_DIGEST_SIZES, "Digest Size");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
