using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static readonly string[] VALID_HASH_ALG =
        {
            "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256", "sha3-224",
            "sha3-256", "sha3-384", "sha3-512"
        };

        public static readonly string[] VALID_MODES = EnumHelpers.GetEnumDescriptions<AnsiX942Types>().ToArray();
        public static readonly int MIN_KEY_LEN = 1;
        public static readonly int MAX_KEY_LEN = 65536;
        public static readonly int MIN_ZZ_LEN = 1;
        public static readonly int MAX_ZZ_LEN = 65536;
        public static readonly int MIN_OTHER_INFO_LEN = 0;
        public static readonly int MAX_OTHER_INFO_LEN = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("ansix9.42", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            string result;
            result = ValidateArray(parameters.KdfType, VALID_MODES, "KDF Mode");
            errors.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.HashAlg, VALID_HASH_ALG, "Hash Alg");
            errors.AddIfNotNullOrEmpty(result);

            ValidateDomain(parameters.ZzLen, errors, "ZzLen", MIN_ZZ_LEN, MAX_ZZ_LEN);
            ValidateDomain(parameters.OtherInfoLen, errors, "OtherInfoLen", MIN_OTHER_INFO_LEN, MAX_OTHER_INFO_LEN);
            ValidateDomain(parameters.KeyLen, errors, "KeyLen", MIN_KEY_LEN, MAX_KEY_LEN);

            return new ParameterValidateResponse(errors);
        }
    }
}
