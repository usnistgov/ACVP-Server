using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = {"sha1", "sha2"};
        public static string[] VALID_DIGEST_SIZES = {"160", "224", "256", "384", "512", "512/224", "512/256"};
        public static string[] VALID_SHA1_SIZES = {"160"};
        public static string[] VALID_SHA2_SIZES = {"224", "256", "384", "512", "512/224", "512/256"};

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            //ValidateBooleanProperties(parameters, errorResults);
            ValidateFunctions(parameters, errorResults);
            ValidateMatching(parameters, errorResults);
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        // The input should only be "yes" or "no" 
        //private void ValidateBooleanProperties(Parameters parameters, List<string> errorResults)
        //{
        //    if (parameters.BitOriented != "yes" && parameters.BitOriented != "no")
        //    {
        //        errorResults.Add("Bad inBit, must be yes/no");
        //    }

        //    if (parameters.IncludeNull != "yes" && parameters.IncludeNull != "no")
        //    {
        //        errorResults.Add("Bad inEmpty, must be yes/no");
        //    }
        //}

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_MODES, "SHA Function");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.DigestSizes, VALID_DIGEST_SIZES, "Digest Size");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateMatching(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Algorithm.ToLower() == "sha1")
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA1_SIZES, "SHA1 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
            else if (parameters.Algorithm.ToLower() == "sha2")
            {
                string result = ValidateArray(parameters.DigestSizes, VALID_SHA2_SIZES, "SHA2 digest size");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }
    }
}
