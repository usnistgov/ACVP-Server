using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = ShaAttributes.GetShaNames().Except(new[] { "SHAKE-128", "SHAKE-256" }).ToArray();
        public static int MIN_KEY_LEN = 112;
        public static int MAX_KEY_LEN = 4096;
        public static int MIN_PASS_LEN = 8;
        public static int MAX_PASS_LEN = 128;
        public static int MIN_SALT_LEN = 128;
        public static int MAX_SALT_LEN = 4096;
        public static int MIN_ITR_COUNT = 1;
        public static int MAX_ITR_COUNT = 10000000;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("pbkdf", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
            {
                return new ParameterValidateResponse(errors);
            }

            foreach (var capability in parameters.Capabilities)
            {
                var results = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(results);

                ValidateDomain(capability.KeyLength, errors, "Key Len", MIN_KEY_LEN, MAX_KEY_LEN);
                ValidateDomain(capability.SaltLength, errors, "Salt Len", MIN_SALT_LEN, MAX_SALT_LEN);
                ValidateDomain(capability.PasswordLength, errors, "Password Len", MIN_PASS_LEN, MAX_PASS_LEN);
                ValidateDomain(capability.IterationCount, errors, "Iteration Count", MIN_ITR_COUNT, MAX_ITR_COUNT);
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
