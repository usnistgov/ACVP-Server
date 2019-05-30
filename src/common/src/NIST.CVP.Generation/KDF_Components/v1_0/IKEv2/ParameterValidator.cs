using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.IKEv2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = {"sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512"};
        public static int MIN_NONCE = 64;
        public static int MAX_NONCE = 2048;
        public static int MIN_DKM = 160;
        public static int MAX_DKM = 16384;
        public static int MIN_DH = 224;
        public static int MAX_DH = 8192;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("ikev2", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }


            if (parameters.Capabilities.Length == 0)
            {
                errors.Add("No capabilties provided");
            }


            foreach (var capability in parameters.Capabilities)
            {

                var result = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(result);

                if (errors.Count > 0)
                {
                    return new ParameterValidateResponse(errors);
                }

                ValidateDomain(capability.InitiatorNonceLength, errors, "NInit", MIN_NONCE, MAX_NONCE);
                ValidateDomain(capability.ResponderNonceLength, errors, "NResp", MIN_NONCE, MAX_NONCE);
                ValidateDomain(capability.DiffieHellmanSharedSecretLength, errors, "DH", MIN_DH, MAX_DH);
                ValidateDomain(capability.DerivedKeyingMaterialLength, errors, "DKM", MIN_DKM, MAX_DKM);
                ValidateDKM(capability.DerivedKeyingMaterialLength, capability.HashAlg, errors, "DKM");
            }

            return new ParameterValidateResponse(errors);
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

        private void ValidateDKM(MathDomain dkm, string[] hashAlgs, List<string> errors, string errorTag)
        {
            var dkmMax = dkm.GetDomainMinMax().Maximum;
            foreach (var hash in hashAlgs)
            {
                var hashOutLen = ShaAttributes.GetHashFunctionFromName(hash).OutputLen;

                if (dkmMax < hashOutLen)
                {
                    errors.Add("Largest DKM value must be greater than or equal to the length of the hashes");
                }
            }
        }
    }
}
