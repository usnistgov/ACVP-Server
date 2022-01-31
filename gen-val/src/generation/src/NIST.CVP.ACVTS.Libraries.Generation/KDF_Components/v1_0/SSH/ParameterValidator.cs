using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" };
        public static string[] VALID_CIPHERS = EnumHelpers.GetEnumDescriptions<Cipher>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("ssh", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            string result;
            result = ValidateArray(parameters.Cipher, VALID_CIPHERS, "Ciphers");
            errors.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.HashAlg, VALID_HASH_ALGS, "Hash Algs");
            errors.AddIfNotNullOrEmpty(result);

            return new ParameterValidateResponse(errors);
        }
    }
}
