using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072, 4096 };
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            if (parameters.PubExpMode == PublicExponentModes.Invalid)
            {
                errorResults.Add("Public exponent mode not found");
            }

            if (parameters.KeyFormat == PrivateKeyModes.Invalid)
            {
                errorResults.Add("Invalid or no private key format provided");
            }

            if (parameters.PubExpMode == PublicExponentModes.Fixed)
            {
                if (parameters.FixedPubExp == null)
                {
                    errorResults.Add("Invalid or no fixed public exponent provided");
                }
                else
                {
                    try
                    {
                        PrimeGeneratorGuard.AgainstInvalidPublicExponent(parameters.FixedPubExp.ToPositiveBigInteger());
                    }
                    catch (RsaPrimeGenException e)
                    {
                        errorResults.Add(e.Message);
                    }
                }
            }

            if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.AlgSpecs, "capabilities")))
            {
                return new ParameterValidateResponse(errorResults);
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(algSpec.Capabilities, "properties")))
                {
                    return new ParameterValidateResponse(errorResults);
                }

                if (algSpec.RandPQ == PrimeGenFips186_4Modes.Invalid)
                {
                    errorResults.Add("Invalid or no rand pq");
                }

                if (errorResults.Count > 0)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    errorResults.AddIfNotNullOrEmpty(ValidateArray(new[] { capability.Modulo }, VALID_MODULI, "modulo"));

                    if (algSpec.RandPQ == PrimeGenFips186_4Modes.B32 || algSpec.RandPQ == PrimeGenFips186_4Modes.B34 || algSpec.RandPQ == PrimeGenFips186_4Modes.B35)
                    {
                        var result = ValidateArray(capability.HashAlgs, VALID_HASH_ALGS, "Hash Alg");
                        errorResults.AddIfNotNullOrEmpty(result);
                    }

                    if (algSpec.RandPQ == PrimeGenFips186_4Modes.B33 || algSpec.RandPQ == PrimeGenFips186_4Modes.B35 || algSpec.RandPQ == PrimeGenFips186_4Modes.B36)
                    {
                        if (capability.PrimeTests.Contains(PrimeTestFips186_4Modes.Invalid) || !capability.PrimeTests.Any())
                        {
                            errorResults.Add("Invalid prime test provided");
                        }
                    }
                }
            }

            return new ParameterValidateResponse(errorResults);
        }
    }
}
