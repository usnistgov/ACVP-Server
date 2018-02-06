using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = {2048, 3072};
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };
        public static string[] VALID_KEY_GEN_MODES = EnumHelpers.GetEnumDescriptions<PrimeGenModes>().ToArray();
        public static string[] VALID_PUB_EXP_MODES = EnumHelpers.GetEnumDescriptions<PublicExponentModes>().ToArray();
        public static string[] VALID_PRIME_TESTS = EnumHelpers.GetEnumDescriptions<PrimeTestModes>().ToArray();
        public static string[] VALID_KEY_FORMATS = EnumHelpers.GetEnumDescriptions<PrivateKeyModes>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            var result = "";

            result = ValidateValue(parameters.PubExpMode, VALID_PUB_EXP_MODES, "PubExp Modes");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateHex(parameters.FixedPubExp, "FixedPubExp hex");
            errorResults.AddIfNotNullOrEmpty(result);

            result = ValidateValue(parameters.KeyFormat, VALID_KEY_FORMATS, "Private Key Format");
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
