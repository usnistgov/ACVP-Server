using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using System.Numerics;

namespace NIST.CVP.Generation.AES_ECB
{
    

    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;


        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);

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
    }
}
