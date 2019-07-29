using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SRTP
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_AES_KEY_LENGTHS = {128, 192, 256};
        public static int[] VALID_KDR_EXPONENTS = Enumerable.Range(1, 24).ToArray();    // 1-24

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("srtp", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }
            
            string result;
            result = ValidateArray(parameters.AesKeyLength, VALID_AES_KEY_LENGTHS, "AES Key Lengths");
            errors.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.KdrExponent, VALID_KDR_EXPONENTS, "KDR Exponents");
            errors.AddIfNotNullOrEmpty(result);

            return new ParameterValidateResponse(errors);
        }
    }
}
