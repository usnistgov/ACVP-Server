using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072 };
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha-224", "sha-256", "sha-384", "sha-512", "sha-512/224", "sha-512/256" };
        public static string[] VALID_SIG_GEN_MODES = { "ansx9.31", "pkcs1v1.5", "pss" };
        public static Dictionary<string, int> VALID_MAX_SALT_LEN = new Dictionary<string, int>();
        
        public ParameterValidator()
        {
            for(var i = 0; i < VALID_HASH_ALGS.Length; i++)
            {
                var maxLen = 0;
                switch (VALID_HASH_ALGS[i])
                {
                    case "sha-1":
                        maxLen = 20;
                        break;

                    case "sha-224":
                    case "sha-512/224":
                        maxLen = 28;
                        break;

                    case "sha-256":
                    case "sha-512/256":
                        maxLen = 32;
                        break;

                    case "sha-384":
                        maxLen = 48;
                        break;

                    case "sha-512":
                        maxLen = 64;
                        break;
                }

                VALID_MAX_SALT_LEN[VALID_HASH_ALGS[i]] = maxLen;
            }
        }

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            var result = ValidateArray(parameters.Moduli, VALID_MODULI, "Modulo");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            if(parameters.Capabilities.Length == 0)
            {
                errorResults.Add("No Capabilities found");
            }

            for(var i = 0; i < parameters.Capabilities.Length; i++)
            {
                var capability = parameters.Capabilities[i];

                result = ValidateValue(capability.HashAlg, VALID_HASH_ALGS, "Hash Algorithms");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                    continue;
                }

                int maxSaltLen;
                VALID_MAX_SALT_LEN.TryGetValue(capability.HashAlg, out maxSaltLen);
                result = ValidateRange(new[] { capability.SaltLen }, 0, maxSaltLen, "Salt length");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }

            result = ValidateArray(parameters.SigGenModes, VALID_SIG_GEN_MODES, "SigGen Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }
    }
}
