using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" };
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

            if (errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
            {
                return new ParameterValidateResponse(errors);
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
