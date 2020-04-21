using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen 
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = PrimeGeneratorGuard.ValidModulusFips186_5;
        public static string[] VALID_HASH_ALGS = ShaAttributes.GetShaNames().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            if (parameters.PubExpMode == PublicExponentModes.Invalid)
            {
                errorResults.Add("Invalid or no public exponent mode provided");
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
                if (algSpec.RandPQ == PrimeGenModes.Invalid)
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
                        PrimeGeneratorGuard.AgainstInvalidModulusFips186_5(capability.Modulo);
                    }
                    catch (RsaPrimeGenException e)
                    {
                        errorResults.Add(e.Message);
                    }
                    
                    if (algSpec.RandPQ == PrimeGenModes.RandomProvablePrimes || algSpec.RandPQ == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes || algSpec.RandPQ == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes)
                    {
                        var result = ValidateArray(capability.HashAlgs, VALID_HASH_ALGS, "Hash Alg");
                        errorResults.AddIfNotNullOrEmpty(result);
                    }

                    if (algSpec.RandPQ == PrimeGenModes.RandomProbablePrimes || algSpec.RandPQ == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes || algSpec.RandPQ == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes)
                    {
                        if (capability.PrimeTests.Contains(PrimeTestModes.Invalid) || !capability.PrimeTests.Any())
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