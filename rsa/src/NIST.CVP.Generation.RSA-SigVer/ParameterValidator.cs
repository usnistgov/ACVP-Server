using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Math;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] VALID_MODULI = { 1024, 2048, 3072 };
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha-224", "sha-256", "sha-384", "sha-512", "sha-512/224", "sha-512/256" };
        public static string[] VALID_SIG_GEN_MODES = { "ansx9.31", "pkcs1v1.5", "pss" };
        public static string[] VALID_PUB_EXP_MODES = { "fixed", "random" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            var result = ValidateArray(parameters.SigVerModes, VALID_SIG_GEN_MODES, "SigVer Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateValue(parameters.PubExpMode, VALID_PUB_EXP_MODES, "Public Exponent Modes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            else
            {
                if (parameters.PubExpMode == "fixed")
                {
                    result = ValidateHex(parameters.FixedPubExpValue, "Fixed Public Exponent Value");
                    if (!string.IsNullOrEmpty(result))
                    {
                        errorResults.Add(result);
                    }
                    else
                    {
                        // Check range for E value
                        var eValue = new BitString(parameters.FixedPubExpValue).ToPositiveBigInteger();
                        if (eValue < NumberTheory.Pow2(16) || eValue > NumberTheory.Pow2(256) || eValue.IsEven)
                        {
                            errorResults.Add("Improper E value provided");
                        }
                    }
                }
            }

            if(parameters.Capabilities.Length == 0)
            {
                errorResults.Add("No Capabilities found");
            }

            foreach (var capability in parameters.Capabilities)
            {
                result = ValidateValue(capability.Modulo, VALID_MODULI, "Modulo");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }

                if(capability.HashPairs.Length == 0)
                {
                    errorResults.Add("No Hash Pairs found");
                }

                foreach (var hashPair in capability.HashPairs)
                {
                    result = ValidateValue(hashPair.HashAlg, VALID_HASH_ALGS, "Hash Algorithms");
                    if (!string.IsNullOrEmpty(result))
                    {
                        errorResults.Add(result);
                        continue;
                    }

                    result = ValidateSaltLen(hashPair.SaltLen, hashPair.HashAlg, capability.Modulo);
                    if (!string.IsNullOrEmpty(result))
                    {
                        errorResults.Add(result);
                    }
                }
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private string ValidateSaltLen(int saltLen, string hashAlg, int modulo)
        {
            if(saltLen < 0)
            {
                return "Salt Length must be positive value";
            }

            var hashFunction = SHAEnumHelpers.StringToHashFunction(hashAlg);
            var digestSize = SHAEnumHelpers.DigestSizeToInt(hashFunction.DigestSize);
            var maxSaltLen = digestSize / 8;

            // Special case for SHA-512 and 1024-bit RSA
            if(modulo == 1024 && maxSaltLen == 64)
            {
                maxSaltLen = 62;
            }

            if(saltLen > maxSaltLen)
            {
                return $"Salt Length must be below max value of {maxSaltLen} for given parameters";
            }

            return "";
        }
    }
}
