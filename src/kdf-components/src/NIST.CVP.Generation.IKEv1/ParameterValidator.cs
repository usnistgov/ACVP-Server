using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv1
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_AUTH_METHODS = EnumHelpers.GetEnumDescriptions<AuthenticationMethods>().ToArray();
        public static string[] VALID_HASH_ALGS = {"sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512"};
        public static int MIN_NONCE = 64;
        public static int MAX_NONCE = 2048;
        public static int MIN_PRESHARED_KEY = 8;
        public static int MAX_PRESHARED_KEY = 8192;
        public static int MIN_DH = 224;
        public static int MAX_DH = 8192;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("ikev1", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            foreach (var capability in parameters.Capabilities)
            {
                string result;
                result = ValidateValue(capability.AuthenticationMethod, VALID_AUTH_METHODS, "Authentication Method");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(result);

                ValidateDomain(capability.InitiatorNonceLength, errors, "NInit", MIN_NONCE, MAX_NONCE);
                ValidateDomain(capability.ResponderNonceLength, errors, "NResp", MIN_NONCE, MAX_NONCE);
                ValidateDomain(capability.DiffieHellmanSharedSecretLength, errors, "DH", MIN_DH, MAX_DH);

                if (capability.AuthenticationMethod.Equals("psk", StringComparison.OrdinalIgnoreCase))
                {
                    ValidateDomain(capability.PreSharedKeyLength, errors, "PreSharedKey", MIN_PRESHARED_KEY, MAX_PRESHARED_KEY);
                }
            }

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
    }
}
