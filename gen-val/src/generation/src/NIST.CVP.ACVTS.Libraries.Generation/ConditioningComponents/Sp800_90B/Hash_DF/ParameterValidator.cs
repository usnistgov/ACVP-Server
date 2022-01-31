using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.Hash_DF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_INPUT_LEN = 1;
        public static int MAX_INPUT_LEN = 65536;
        public static string[] VALID_HASH_ALG = ShaAttributes.GetShaNames().Except(new[] { "sha3-224", "sha3-256", "sha3-384", "sha3-512", "shake-128", "shake-256" }, StringComparer.OrdinalIgnoreCase).ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            foreach (var capability in parameters.Capabilities)
            {
                ValidateInputLengths(capability, errorResults);
                ValidateHashAlgs(capability, errorResults);
            }

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateInputLengths(Capabilities capability, List<string> errorResults)
        {
            ValidateDomain(capability.PayloadLen, errorResults, "payloadLen", MIN_INPUT_LEN, MAX_INPUT_LEN);
        }

        private void ValidateHashAlgs(Capabilities capability, List<string> errorResults)
        {
            var results = ValidateArray(capability.HashAlg, VALID_HASH_ALG, "hashAlg");
            errorResults.AddIfNotNullOrEmpty(results);
        }
    }
}
