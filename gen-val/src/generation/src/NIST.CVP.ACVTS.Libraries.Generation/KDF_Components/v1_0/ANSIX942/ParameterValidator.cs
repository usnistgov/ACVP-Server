using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static readonly string[] VALID_HASH_ALG =
        {
            "SHA-1",
            "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256",
            "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512"
        };

        public static readonly string[] VALID_MODES = EnumHelpers.GetEnumDescriptions<AnsiX942Types>().ToArray();
        public static readonly int MIN_KEY_LEN = 8;
        public static readonly int MAX_KEY_LEN = 4096;
        public static readonly int MIN_ZZ_LEN = 8;
        public static readonly int MAX_ZZ_LEN = 4096;
        public static readonly int MIN_OTHER_INFO_LEN = 0;
        public static readonly int MAX_OTHER_INFO_LEN = 4096;     // Same for suppLen

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
            result = ValidateArray(parameters.HashAlg, VALID_HASH_ALG, "Hash Alg");
            errors.AddIfNotNullOrEmpty(result);

            ValidateDomain(parameters.ZzLen, errors, "ZzLen", MIN_ZZ_LEN, MAX_ZZ_LEN);
            ValidateMultipleOf(parameters.ZzLen, errors, 8, "ZzLen Modulus 8");

            result = ValidateArray(parameters.KdfType, EnumHelpers.GetEnumsWithoutDefault<AnsiX942Types>(), "KdfType");
            errors.AddIfNotNullOrEmpty(result);

            if (parameters.KdfType.Contains(AnsiX942Types.Concat))
            {
                ValidateDomain(parameters.OtherInfoLen, errors, "OtherInfoLen", MIN_OTHER_INFO_LEN, MAX_OTHER_INFO_LEN);
                ValidateMultipleOf(parameters.OtherInfoLen, errors, 8, "OtherInfo Modulus 8");
            }

            if (parameters.KdfType.Contains(AnsiX942Types.Der))
            {
                ValidateDomain(parameters.SuppInfoLen, errors, "SuppInfoLen", MIN_OTHER_INFO_LEN, MAX_OTHER_INFO_LEN);
                ValidateMultipleOf(parameters.SuppInfoLen, errors, 8, "SuppInfoLen Modulus 8");

                result = ValidateArray(parameters.Oid, EnumHelpers.GetEnumsWithoutDefault<AnsiX942Oids>(), "OID");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArrayHasNoDuplicates(parameters.Oid, "OID");
                errors.AddIfNotNullOrEmpty(result);
            }

            ValidateDomain(parameters.KeyLen, errors, "KeyLen", MIN_KEY_LEN, MAX_KEY_LEN);
            ValidateMultipleOf(parameters.KeyLen, errors, 8, "KeyLen Modulus 8");

            return new ParameterValidateResponse(errors);
        }
    }
}
