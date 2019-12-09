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

            if (!parameters.Capabilities.Any())
            {
                errors.Add("Must have capabilities");
                return new ParameterValidateResponse(errors);
            }

            foreach (var capability in parameters.Capabilities)
            {
                var results = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(results);

                ValidateDomain(capability.KeyLength, MIN_KEY_LEN, MAX_KEY_LEN, errors, "Key Len");
                ValidateDomain(capability.SaltLength, MIN_SALT_LEN, MAX_SALT_LEN, errors, "Salt Len");
                ValidateDomain(capability.PasswordLength, MIN_PASS_LEN, MAX_PASS_LEN, errors, "Password Len");

                // If the intersection of the FAST and the selected is equal to the selected
                //     then there are only FAST hash algs present
                var maxItrCount =
                    capability.HashAlg.Intersect(FAST_HASH_ALGS, StringComparer.OrdinalIgnoreCase).Count() ==
                    capability.HashAlg.Count()
                        ? MAX_ITR_COUNT_FAST
                        : MAX_ITR_COUNT; 
                
                ValidateDomain(capability.IterationCount, MIN_ITR_COUNT, maxItrCount, errors, "Iteration Count");
            }

            return new ParameterValidateResponse(errors);
        }

        private void ValidateDomain(MathDomain domain, int min, int max, List<string> errors, string friendlyName)
        {
            // Check there are segments
            if (!domain.DomainSegments.Any())
            {
                errors.Add($"No segments found within {friendlyName}");
                return;
            }
            
            // Check min and max
            var minMax = domain.GetDomainMinMax();

            if (minMax.Minimum < min)
            {
                errors.Add($"Minimum {friendlyName} provided is below allowed value of {min}");
            }

            if (minMax.Maximum > max)
            {
                errors.Add($"Maximum {friendlyName} provided is above allowed value of {max}");
            }
        }
    }
}