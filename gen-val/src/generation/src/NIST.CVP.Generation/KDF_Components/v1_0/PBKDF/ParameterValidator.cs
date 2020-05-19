using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = ShaAttributes.GetShaNames().Except(new [] {"SHAKE-128", "SHAKE-256"}).ToArray();
        public static string[] FAST_HASH_ALGS = {"SHA-1", "SHA2-256", "SHA2-384", "SHA2-512"};
        public static int MIN_KEY_LEN = 112;
        public static int MAX_KEY_LEN = 65536;
        public static int MIN_PASS_LEN = 8;
        public static int MAX_PASS_LEN = 128;
        public static int MIN_SALT_LEN = 128;
        public static int MAX_SALT_LEN = 4096;
        public static int MIN_ITR_COUNT = 1;
        public static int MAX_ITR_COUNT = 10000;
        public static int MAX_ITR_COUNT_FAST = 10000000;
        
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("pbkdf", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            if (!errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
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

                // If the intersection of the FAST and the selected is equal to the selected
                //     then there are only FAST hash algs present
                var maxItrCount =
                    capability.HashAlg.Intersect(FAST_HASH_ALGS, StringComparer.OrdinalIgnoreCase).Count() ==
                    capability.HashAlg.Count()
                        ? MAX_ITR_COUNT_FAST
                        : MAX_ITR_COUNT; 
                
                ValidateDomain(capability.IterationCount, errors, "Iteration Count", MIN_ITR_COUNT, maxItrCount);
            }

            return new ParameterValidateResponse(errors);
        }
    }
}