using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072, 4096 };
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512"};
        public static string[] VALID_XOF_ALGS = { "SHAKE-128", "SHAKE-256" };
        public static string[] VALID_HASH_AND_XOF_ALGS = VALID_HASH_ALGS.Concat(VALID_XOF_ALGS).ToArray();
        public static SignatureSchemes[] VALID_SIG_VER_MODES = { SignatureSchemes.Pkcs1v15, SignatureSchemes.Pss };
        public static string[] VALID_CONFORMANCES = { "SP800-106" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            var result = "";

            if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "capabilities")))
            {
                return new ParameterValidateResponse(errorResults);
            }

            foreach (var capability in parameters.Capabilities)
            {
                if (capability.ModuloCapabilities.Length == 0)
                {
                    errorResults.Add("No capabilities listed for a SigVer mode");
                    continue;
                }

                if (!VALID_SIG_VER_MODES.Contains(capability.SigType))
                {
                    errorResults.Add("Invalid signature scheme found");
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

                    if (capability.SigType == SignatureSchemes.Pss)
                    {
                        if (moduloCap.MaskFunction.Length == 0)
                        {
                            errorResults.Add("No mask generation function found");
                        }

                        if (moduloCap.MaskFunction.Contains(PssMaskTypes.None))
                        {
                            errorResults.Add("Unable to resolve a mask generation function");
                        }
                    }

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        if (hashPair.HashAlg.Length == 0)
                        {
                            errorResults.Add("No hash functions listed within a HashPair");
                            continue;
                        }

                        if (capability.SigType == SignatureSchemes.Pss) // XOFs are only valid for the "Hash" for PSS
                        {
                            result = ValidateValue(hashPair.HashAlg, VALID_HASH_AND_XOF_ALGS, "Hash Algorithms");    
                        }
                        else
                        {
                            result = ValidateValue(hashPair.HashAlg, VALID_HASH_ALGS, "Hash Algorithms");    
                        }
                        
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                            continue;
                        }

                        result = ValidateSaltLen(hashPair.SaltLen, hashPair.HashAlg);
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                        }
                    }
                }
            }

            if (parameters.PubExpMode == PublicExponentModes.Invalid)
            {
                errorResults.Add("Invalid or no fixed public value mode provided");
            }
            else if (parameters.PubExpMode == PublicExponentModes.Fixed)
            {
                if (parameters.FixedPubExpValue == null)
                {
                    errorResults.Add("Invalid or no fixed public exponent provided");
                }
                else
                {
                    try
                    {
                        PrimeGeneratorGuard.AgainstInvalidPublicExponent(parameters.FixedPubExpValue
                            .ToPositiveBigInteger());
                    }
                    catch (RsaPrimeGenException e)
                    {
                        errorResults.Add(e.Message);
                    }
                }
            }

            ValidateConformances(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private string ValidateSaltLen(int saltLen, string hashAlg)
        {
            if (saltLen < 0)
            {
                return "Salt Length must be positive value";
            }

            // Use hash function to compute max allowed salt length
            int outputLen;
            if (!VALID_XOF_ALGS.Contains(hashAlg))
            {
                outputLen = ShaAttributes.GetHashFunctionFromName(hashAlg).OutputLen;    
            }
            else
            {
                outputLen = ShaAttributes.GetXofPssHashFunctionFromName(hashAlg).OutputLen;
            }
            
            var maxSaltLen = outputLen / 8;

            if (saltLen > maxSaltLen)
            {
                return $"Salt Length must be less than or equal to the max value of {maxSaltLen} for given parameters";
            }

            return "";
        }

        private void ValidateConformances(Parameters parameters, List<string> errors)
        {
            if (parameters.Conformances != null && parameters.Conformances.Length != 0)
            {
                var result = ValidateArray(parameters.Conformances, VALID_CONFORMANCES, "Conformances");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }
            }
        }
    }
}
