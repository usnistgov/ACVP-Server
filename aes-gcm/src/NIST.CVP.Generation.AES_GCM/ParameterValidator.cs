using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using System.Numerics;

namespace NIST.CVP.Generation.AES_GCM
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 32, 64, 96, 104, 112, 120, 128 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static string[] VALID_IV_GEN = new string[] { "internal", "external" };
        public static string[] VALID_IV_GEN_MODE = new string[] { "8.2.1", "8.2.2" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 65536;
        public static int VALID_MIN_IV = 8;
        public static int VALID_MAX_IV = 1024;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateIV(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidatePlainText(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.PtLen, VALID_MIN_PT, VALID_MAX_PT, "Plaintext length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.PtLen, 8, "Plaintext length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
        
        private void ValidateKeySizes(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDirection(Parameters parameters,  List<string> errorResults)
        {
            string result = ValidateArray(parameters.Mode, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateTagSizes(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateArray(parameters.TagLen, VALID_TAG_LENGTHS, "Tag Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateAAD(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.aadLen, VALID_MIN_AAD, VALID_MAX_AAD, "AAD length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.aadLen, 8, "AAD length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateIV(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.ivLen, VALID_MIN_IV, VALID_MAX_IV, "IV Length (range)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.ivLen, 8, "IV length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateValue(parameters.ivGen, VALID_IV_GEN, "IV Generation");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            // Only validate ivGenMode when ivGen is not null and is internal
            if (!string.IsNullOrEmpty(parameters.ivGen) && parameters.ivGen.Equals("internal", StringComparison.CurrentCultureIgnoreCase))
            {
                result = ValidateValue(parameters.ivGenMode, VALID_IV_GEN_MODE, "IV Generation Mode (Internal)");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }


    }
}
