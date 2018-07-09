using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

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

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            // Gracefully check if the public exponent is valid
            // Already checked if it was valid hex, so this is safe to run
            if (EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode) == PublicExponentModes.Fixed)
            {
                var eValue = new BitString(parameters.FixedPubExp).ToPositiveBigInteger();
                if (eValue <= NumberTheory.Pow2(16) || eValue >= NumberTheory.Pow2(256) || eValue.IsEven)
                {
                    errorResults.Add("Invalid public exponent value provided");
                }
            }

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

                if (errorResults.Count > 0)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    result = ValidateValue(capability.Modulo, VALID_MODULI, "Modulo");
                    errorResults.AddIfNotNullOrEmpty(result);

                    var friendlyName = algSpec.RandPQ.ToLower();

                    if (friendlyName == "b.3.2" || friendlyName == "b.3.4" || friendlyName == "b.3.5")
                    {
                        result = ValidateArray(capability.HashAlgs, VALID_HASH_ALGS, "Hash Alg");
                        errorResults.AddIfNotNullOrEmpty(result);
                    }

                    if (friendlyName == "b.3.3" || friendlyName == "b.3.5" || friendlyName == "b.3.6")
                    {
                        result = ValidateArray(capability.PrimeTests, VALID_PRIME_TESTS, "Prime Tests");
                        errorResults.AddIfNotNullOrEmpty(result);
                    }
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
