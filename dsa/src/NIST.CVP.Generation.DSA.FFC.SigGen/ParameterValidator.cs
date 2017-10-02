using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_L = { 2048, 3072 };
        public static int[] VALID_N = { 224, 256 };
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            foreach (var capability in parameters.Capabilities)
            {
                result = ValidateValue(capability.L, VALID_L, "L");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateValue(capability.N, VALID_N, "N");
                errors.AddIfNotNullOrEmpty(result);

                if (!VerifyLenPair(capability.L, capability.N))
                {
                    errors.Add("Invalid L, N pair");
                }

                result = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(result);
            }

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }

        private bool VerifyLenPair(int L, int N)
        {
            if (L == 2048)
            {
                return (N == 224 || N == 256);
            }
            else if (L == 3072)
            {
                return (N == 256);
            }

            return false;
        }
    }
}
