using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "SHA2-256", "SHA2-384", "SHA2-512" };
        public static int VALID_MAX_KEY_BLOCK_LENGTH = 1024;
        public static int VALID_MIN_KEY_BLOCK_LENGTH = 512;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            ValidateAlgoMode(parameters, new[] { AlgoMode.Tls_v1_2_RFC7627 }, errors);
            errors.AddIfNotNullOrEmpty(ValidateArray(parameters.HashAlg, VALID_HASH_ALGS, "Hash Algs"));

            parameters.TlsVersion = new[] { TlsModes.v12_extendedMasterSecret };
            
            ValidateDomain(parameters.KeyBlockLength, errors, "Key Block Length", VALID_MIN_KEY_BLOCK_LENGTH, VALID_MAX_KEY_BLOCK_LENGTH);
            ValidateMultipleOf(parameters.KeyBlockLength, errors, 8, "Key Block Length multiple of 8");
            
            return new ParameterValidateResponse(errors);
        }
    }
}
