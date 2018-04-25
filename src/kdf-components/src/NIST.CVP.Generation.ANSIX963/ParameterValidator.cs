using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ANSIX963
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_FIELD_SIZE = {224, 233, 256, 283, 384, 409, 521, 571};
        public static int SHARED_INFO_MAXIMUM = 1024;
        public static int SHARED_INFO_MINIMUM = 0;
        public static string[] VALID_HASH_ALGS = {"sha2-224", "sha2-256", "sha2-384", "sha2-512"};
        public static int KEY_LENGTH_MINIMUM = 1;
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

            if (parameters.FieldSize.Length != 1 && parameters.FieldSize.Length != 2)
            {
                errors.Add("Must contain 1 or 2 field sizes");
            }

            result = ValidateArray(parameters.FieldSize, VALID_FIELD_SIZE, "Field Size");
            errors.AddIfNotNullOrEmpty(result);

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            ValidateGroups(parameters.FieldSize, parameters.HashAlg, errors);

            ValidateDomain(parameters.KeyDataLength, errors, "KeyDataLength", KEY_LENGTH_MINIMUM, KEY_LENGTH_MAXIMUM);
            ValidateDomain(parameters.SharedInfoLength, errors, "SharedInfo", SHARED_INFO_MINIMUM, SHARED_INFO_MAXIMUM);

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateDomain(MathDomain domain, List<string> errors, string errorTag, int min, int max)
        {
            var result = ValidateSegmentCountGreaterThanZero(domain, errorTag);
            if (!string.IsNullOrEmpty(result))
            {
                errors.Add(result);
                return;
            }

            if (domain.GetDomainMinMax().Minimum < min)
            {
                errors.Add($"Minimum {errorTag} must be greater than or equal to {min}");
            }

            if (domain.GetDomainMinMax().Maximum > max)
            {
                errors.Add($"Maximum {errorTag} must be less than or equal to {max}");
            }
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
