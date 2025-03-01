﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_L = { 1024, 2048, 3072 };
        public static int[] VALID_N = { 160, 224, 256 };
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256" };
        public static string[] VALID_PQ_MODES = { "probable", "provable" };
        public static string[] VALID_G_MODES = { "canonical", "unverifiable" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            if (errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
            {
                return new ParameterValidateResponse(errors);
            }

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

                if (capability.PQGen.Length == 0 || capability.GGen.Length == 0)
                {
                    errors.Add("No PQ Mode or G Mode found");
                    continue;
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

                result = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }

                foreach (var hashAlg in capability.HashAlg)
                {
                    if (!VerifyHash(capability.N, hashAlg))
                    {
                        errors.Add($"Invalid hash function for this L, N pair: {capability.N}, {hashAlg}");
                    }
                }
            }

            return new ParameterValidateResponse(errors);
        }

        private bool VerifyHash(int N, string hash)
        {
            if (N == 160)
            {
                // All
                return true;
            }
            if (N == 224)
            {
                // All but SHA-1
                return (hash != "sha-1");
            }
            else if (N == 256)
            {
                // All but SHA-1, SHA2-224, SHA2-512/224
                return (hash != "sha-1" && hash != "SHA2-224" && hash != "SHA2-512/224");
            }

            return false;
        }

        private bool VerifyLenPair(int L, int N)
        {
            if (L == 1024)
            {
                return (N == 160);
            }
            else if (L == 2048)
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
