using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
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
            string result = "";

            result = ValidateValue(parameters.PubExpMode, VALID_PUB_EXP_MODES, "PubExp Modes");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateHex(parameters.FixedPubExp, "FixedPubExp hex");
            errorResults.AddIfNotNullOrEmpty(result);

            if (parameters.AlgSpecs.Length == 0)
            {
                errorResults.Add("Nothing registered");
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                result = ValidateValue(algSpec.RandPQ, VALID_KEY_GEN_MODES, "KeyGen Modes");
                errorResults.AddIfNotNullOrEmpty(result);

                if (algSpec.Capabilities.Length == 0)
                {
                    errorResults.Add("No capabilities listed for a KeyGen mode");
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    result = ValidateValue(capability.Modulo, VALID_MODULI, "Modulo");
                    errorResults.AddIfNotNullOrEmpty(result);

                    result = ValidateArray(capability.HashAlgs, VALID_HASH_ALGS, "Hash Alg");
                    errorResults.AddIfNotNullOrEmpty(result);

                    result = ValidateArray(capability.PrimeTests, VALID_PRIME_TESTS, "Prime Tests");
                    errorResults.AddIfNotNullOrEmpty(result);
                }
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }
    }
}
