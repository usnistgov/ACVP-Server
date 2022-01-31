using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_AUTH_METHODS = EnumHelpers.GetEnumDescriptions<AuthenticationMethods>().ToArray();
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" };
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

            return new ParameterValidateResponse(errors);
        }
    }
}
