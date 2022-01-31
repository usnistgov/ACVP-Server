using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_FIELD_SIZE = { 224, 233, 256, 283, 384, 409, 521, 571 };
        public static int SHARED_INFO_MAXIMUM = 1024;
        public static int SHARED_INFO_MINIMUM = 0;
        public static string[] VALID_HASH_ALGS = { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" };
        public static int KEY_LENGTH_MINIMUM = 112;
        public static int KEY_LENGTH_MAXIMUM = 4096;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("ansix9.63", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            string result;
            result = ValidateArray(parameters.HashAlg, VALID_HASH_ALGS, "Hash Algs");
            errors.AddIfNotNullOrEmpty(result);

            if (parameters.FieldSize == null)
            {
                errors.Add("Field Size must be provided.");
                return new ParameterValidateResponse(errors);
            }

            if (parameters.FieldSize.Length != 1 && parameters.FieldSize.Length != 2)
            {
                errors.Add("Must contain 1 or 2 field sizes");
            }

            result = ValidateArray(parameters.FieldSize, VALID_FIELD_SIZE, "Field Size");
            errors.AddIfNotNullOrEmpty(result);

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateGroups(parameters.FieldSize, parameters.HashAlg, errors);

            ValidateDomain(parameters.KeyDataLength, errors, "KeyDataLength", KEY_LENGTH_MINIMUM, KEY_LENGTH_MAXIMUM);
            ValidateDomain(parameters.SharedInfoLength, errors, "SHAredInfo", SHARED_INFO_MINIMUM, SHARED_INFO_MAXIMUM);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateGroups(int[] fieldSize, string[] hashAlgs, List<string> errors)
        {
            foreach (var field in fieldSize)
            {
                var validGroups = 0;
                foreach (var hashAlg in hashAlgs)
                {
                    var hash = ShaAttributes.GetHashFunctionFromName(hashAlg);
                    if (TestGroupGenerator.IsValidGroup(field, hash.OutputLen))
                    {
                        validGroups++;
                    }
                }

                if (validGroups == 0)
                {
                    errors.Add($"Unable to create any groups for field size {field}. Be sure that the hash functions are supported on the specified field sizes");
                }
            }
        }
    }
}
