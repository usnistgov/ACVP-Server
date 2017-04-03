using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_FUNCTIONS = {"sha3", "shake"};

        public static int[] VALID_DIGEST_SIZES = {128, 224, 256, 384, 512};
        public static int[] VALID_SHA3_DIGEST_SIZES = { 224, 256, 384, 512 };
        public static int[] VALID_SHAKE_DIGEST_SIZES = { 128, 256 };

        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65336;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            var result = ValidateArray(parameters.Function, VALID_FUNCTIONS, "SHA3 Function");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.DigestSize, VALID_DIGEST_SIZES, "Digest Size");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            ValidateMatching(parameters, errorResults);

            if (parameters.Function.Contains("shake"))
            {
                result = ValidateRange(new[] { parameters.MinOutputLength }, VALID_MIN_OUTPUT_SIZE, VALID_MAX_OUTPUT_SIZE,
                "Minimum output length");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }

                result = ValidateRange(new[] { parameters.MaxOutputLength }, VALID_MIN_OUTPUT_SIZE, VALID_MAX_OUTPUT_SIZE,
                    "Minimum output length");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }

                if (parameters.MinOutputLength > parameters.MaxOutputLength)
                {
                    errorResults.Add("Min output length is greater than the max output length");
                }
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();    
        }

        private void ValidateMatching(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Function.Length == parameters.DigestSize.Length)
            {
                for (var i = 0; i < parameters.Function.Length; i++)
                {
                    if (parameters.Function[i].ToLower() == "sha3")
                    {
                        var result = ValidateValue(parameters.DigestSize[i], VALID_SHA3_DIGEST_SIZES,
                            "SHA3 Function with improper digest size");
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                        }
                    }
                    else
                    {
                        var result = ValidateValue(parameters.DigestSize[i], VALID_SHAKE_DIGEST_SIZES,
                            "SHAKE Function with improper digest size");
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                        }
                    }
                }
            }
            else
            {
                errorResults.Add("Each function must have exactly one digest size");
            }
        }
    }
}
