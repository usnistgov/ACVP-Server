using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_AES_KEY_LENGTHS = { 128, 192, 256 };
        public static int[] VALID_KDR_EXPONENTS = Enumerable.Range(0, 25).ToArray();    // 0-24

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

            if (parameters.KdrExponent != null && parameters.KdrExponent.Length != 0)
            {
                result = ValidateArray(parameters.KdrExponent, VALID_KDR_EXPONENTS, "KDR Exponents");
                errors.AddIfNotNullOrEmpty(result);
            }

            if (!parameters.SupportsZeroKdr && (parameters.KdrExponent == null || parameters.KdrExponent.Length == 0))
            {
                errors.Add("Registration must at a minimum support Zero KDR, or provider KDR Exponents.");
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
