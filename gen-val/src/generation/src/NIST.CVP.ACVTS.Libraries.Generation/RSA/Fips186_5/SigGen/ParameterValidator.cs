using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 2048, 3072, 4096 };
        public static string[] VALID_HASH_ALGS = { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512"};
        public static string[] VALID_XOF_ALGS = { "SHAKE-128", "SHAKE-256" };
        public static string[] VALID_HASH_AND_XOF_ALGS = VALID_HASH_ALGS.Concat(VALID_XOF_ALGS).ToArray();
        public static SignatureSchemes[] VALID_SIG_GEN_MODES = { SignatureSchemes.Pkcs1v15, SignatureSchemes.Pss };
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
                if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(capability.ModuloCapabilities, "properties")))
                {
                    continue;
                }

                if (!VALID_SIG_GEN_MODES.Contains(capability.SigType))
                {
                    errorResults.Add("No valid signature scheme found");
                }

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    if (moduloCap.HashPairs == null || moduloCap.HashPairs.Length == 0)
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
                        if (moduloCap.MaskFunction == null || moduloCap.MaskFunction.Length == 0)
                        {
                            errorResults.Add("No mask generation function found");
                            continue;
                        }

                        if (moduloCap.MaskFunction.Contains(PssMaskTypes.None))
                        {
                            errorResults.Add("Unable to resolve a mask generation function");
                        }
                    }
                    else if (capability.SigType == SignatureSchemes.Pkcs1v15 && (moduloCap.MaskFunction.Contains(PssMaskTypes.MGF1) 
                                 || moduloCap.MaskFunction.Contains(PssMaskTypes.SHAKE128) || moduloCap.MaskFunction.Contains(PssMaskTypes.SHAKE256)))
                    {
                        // maskFunction is not a valid property when sigType = pkcs1v1.5. Registrations that supply
                        // maskFunction for pkcs1v1.5 should be rejected. The default value for maskFunction is
                        // PssMaskTypes.None per Parameters.cs. 
                        errorResults.Add($"Mask generation functions are not supported for the {capability.SigType} " +
                                         $"signature type. Remove {nameof(moduloCap.MaskFunction)} from the registration " +
                                         $"for {capability.SigType}.");
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

                        // For Pkcs1v15, hashPairs should not include a saltLen property. W/o restructuring/redoing some  
                        // things, the best check we can do for this is to check for saltLen values that are greater than 0.
                        if (capability.SigType == SignatureSchemes.Pkcs1v15 && hashPair.SaltLen > 0)
                        {
                            errorResults.Add($"{nameof(hashPair.SaltLen)} may not be included within a HashPair for the " +
                                             $"{capability.SigType} signature type.");
                            break;
                        }
                        
                        result = ValidateSaltLen(hashPair.SaltLen, hashPair.HashAlg, moduloCap.Modulo);
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                        }
                    }
                }
            }

            ValidateConformances(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private string ValidateSaltLen(int saltLen, string hashAlg, int modulo)
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
