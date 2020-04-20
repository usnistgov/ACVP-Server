using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = {2048, 3072, 4096};
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };

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
                    // TODO clean up try-catch
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
            
            if (!parameters.AlgSpecs.Any())
            {
                errorResults.Add("Nothing registered");
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (algSpec.RandPQ == PrimeGenFips186_4Modes.Invalid)
                {
                    errorResults.Add("Invalid or no rand pq");
                }

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
                    // TODO clean up try-catch
                    try
                    {
                        PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(capability.Modulo);
                    }
                    catch (RsaPrimeGenException e)
                    {
                        errorResults.Add(e.Message);
                    }
                    
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
