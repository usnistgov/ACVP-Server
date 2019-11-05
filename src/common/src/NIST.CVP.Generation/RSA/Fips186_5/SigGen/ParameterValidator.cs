using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = {2048, 3072, 4096};
        public static string[] VALID_HASH_ALGS = {"sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256"};
        public static SignatureSchemes[] VALID_SIG_GEN_MODES =  {SignatureSchemes.Pkcs1v15, SignatureSchemes.Pss};
        public static string[] VALID_CONFORMANCES = {"SP800-106"};

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            var result = "";

            if (parameters.Capabilities.Length == 0)
            {
                errorResults.Add("Nothing registered");
            }

            foreach (var capability in parameters.Capabilities)
            {
                if (capability.ModuloCapabilities.Length == 0)
                {
                    errorResults.Add("No capabilities listed for a SigGen mode");
                    continue;
                }

                if (!VALID_SIG_GEN_MODES.Contains(capability.SigType))
                {
                    errorResults.Add("No valid signature scheme found");
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

                        result = ValidateValue(hashPair.HashAlg, VALID_HASH_ALGS, "Hash Algorithms");
                        if (!string.IsNullOrEmpty(result))
                        {
                            errorResults.Add(result);
                            continue;
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
            var digestSize = ShaAttributes.GetHashFunctionFromName(hashAlg).OutputLen;
            var maxSaltLen = digestSize / 8;

            if (saltLen > maxSaltLen)
            {
                return $"Salt Length must be below max value of {maxSaltLen} for given parameters";
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