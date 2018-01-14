using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SSH
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = {"sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512"};
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

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
