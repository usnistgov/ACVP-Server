﻿using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072 };
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };
        public static string[] VALID_SIG_GEN_MODES = EnumHelpers.GetEnumDescriptions<SignatureSchemes>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            var result = "";

            if (parameters.Capabilities.Length == 0)
            {
                errorResults.Add("Nothing registered");
            }

            foreach (var capability in parameters.Capabilities)
            {
                if (capability.ModuloCapabilities.Length == 0)
                {
                    errorResults.Add("No capabilities listed for a SigGen mode");
                    continue;
                }

                result = ValidateValue(capability.SigType, VALID_SIG_GEN_MODES, "SigGen Modes");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    if (moduloCap.HashPairs.Length == 0)
                    {
                        errorResults.Add("No hash/salt pairs listed for a modulus");
                        continue;
                    }

                    result = ValidateValue(moduloCap.Modulo, VALID_MODULI, "Modulo");
                    if (!string.IsNullOrEmpty(result))
                    {
                        errorResults.Add(result);
                    }

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        if (hashPair.HashAlg.Length == 0)
                        {
                            errorResults.Add("No hash functions listed within a HashPair");
                            continue;
                        }

                        result = ValidateValue(hashPair.HashAlg, VALID_HASH_ALGS, "Hash Algorithms");
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                            continue;
                        }

                        result = ValidateSaltLen(hashPair.SaltLen, hashPair.HashAlg, moduloCap.Modulo);
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                        }
                    }
                }
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private string ValidateSaltLen(int saltLen, string hashAlg, int modulo)
        {
            if (saltLen < 0)
            {
                return "Salt Length must be positive value";
            }

            // Use hash function to compute max allowed salt length
            var digestSize = ShaAttributes.GetHashFunctionFromName(hashAlg).OutputLen;
            var maxSaltLen = digestSize / 8;

            if (saltLen > maxSaltLen)
            {
                return $"Salt Length must be below max value of {maxSaltLen} for given parameters";
            }

            return "";
        }
    }
}