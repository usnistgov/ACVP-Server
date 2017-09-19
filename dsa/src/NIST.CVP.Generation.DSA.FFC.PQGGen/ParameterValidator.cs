using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_L = { 2048, 3072 };
        public static int[] VALID_N = { 224, 256 };
        public static string[] VALID_HASH_ALGS = { "sha-224", "sha-256", "sha-384", "sha-512", "sha-512/224", "sha-512/256" };
        public static string[] VALID_PQ_MODES = { "probable", "provable" };
        public static string[] VALID_G_MODES = { "canonical", "unverifiable" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            foreach (var capability in parameters.Capabilities)
            {
                result = ValidateValue(capability.L, VALID_L, "L");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                result = ValidateValue(capability.N, VALID_N, "N");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                if (!VerifyLenPair(capability.L, capability.N))
                {
                    errors.Add("Invalid L, N pair");
                }

                result = ValidateArray(capability.PQGen, VALID_PQ_MODES, "PQ Modes");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                result = ValidateArray(capability.GGen, VALID_G_MODES, "G Modes");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                result = ValidateArray(capability.HashAlgs, VALID_HASH_ALGS, "Hash Algs");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                foreach (var hashAlg in capability.HashAlgs)
                {
                    if (!VerifyHash(capability.N, hashAlg))
                    {
                        errors.Add("Invalid hash function for this L, N pair");
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }

        private bool VerifyHash(int N, string hash)
        {
            if (N == 224)
            {
                // All
                return true;
            }
            else if (N == 256)
            {
                // All but SHA2-224, SHA2-512/224
                return (hash != "sha2-224" && hash != "sha2-512/224");
            }

            return false;
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
