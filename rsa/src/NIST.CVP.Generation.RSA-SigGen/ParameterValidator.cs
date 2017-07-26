using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072, 4096 };
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha-224", "sha-256", "sha-384", "sha-512", "sha-512/224", "sha-512/256" };
        public static string[] VALID_SIG_GEN_MODES = { "ansx9.31", "pkcs1v1.5", "pss" };
        public static string[] VALID_SALT_MODES = { "fixed", "random" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            var result = ValidateArray(parameters.Moduli, VALID_MODULI, "Modulo");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.HashAlgs, VALID_HASH_ALGS, "Hash Algorithms");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.SigGenModes, VALID_SIG_GEN_MODES, "SigGen Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateValue(parameters.SaltMode, VALID_SALT_MODES, "Salt Mode");
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
