using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 4, 6, 8, 10, 12, 14, 16 };
        public static int[] VALID_NONCE_LENGTHS = new int[] { 7, 8, 9, 10, 11, 12, 13 };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 32;
        public static int MAX_NUMBER_OF_VALUES_IN_PT_ARRAY = 2;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 32;
        public static int MAX_NUMBER_OF_VALUES_IN_AAD_ARRAY = 2;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateNonce(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateKeySizes(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidatePlainText(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateRange(parameters.PtLen, VALID_MIN_PT, VALID_MAX_PT, "Payload length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateNonce(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.Nonce, VALID_NONCE_LENGTHS, "Nonce Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateAAD(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.AadLen, VALID_MIN_AAD, VALID_MAX_AAD, "AAD length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateTagSizes(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.TagLen, VALID_TAG_LENGTHS, "Tag Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
