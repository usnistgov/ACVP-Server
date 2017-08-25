using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = {2048, 3072};
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha-224", "sha-256", "sha-384", "sha-512", "sha-512/224", "sha-512/256" };
        public static string[] VALID_KEY_GEN_MODES = {"b.3.2", "b.3.3", "b.3.4", "b.3.5", "b.3.6"};
        public static string[] VALID_PUB_EXP_MODES = {"fixed", "random"};
        public static string[] VALID_PRIME_TESTS = {"tblc2", "tblc3"};

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            string result = ValidateArray(parameters.Moduli, VALID_MODULI, "Modulo");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.HashAlgs, VALID_HASH_ALGS, "Hash Algorithms");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.KeyGenModes, VALID_KEY_GEN_MODES, "KeyGen Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.PrimeTests, VALID_PRIME_TESTS, "PrimeTests");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            
            result = ValidateValue(parameters.PubExpMode, VALID_PUB_EXP_MODES, "PubExp Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateHex(parameters.FixedPubExp, "FixedPubExp hex");
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
