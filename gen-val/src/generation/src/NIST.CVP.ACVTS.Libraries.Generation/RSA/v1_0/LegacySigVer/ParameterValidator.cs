using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.LegacySigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // CAVS does not include sha2-224 on the form, I think that is an error in the form because it is in the code
        public static int[] VALID_MODULI = { 1024, 1536, 2048, 3072, 4096 };
        public static string[] VALID_HASH_ALGS = { "SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" };
        public static string[] VALID_SIG_VER_MODES = EnumHelpers.GetEnumDescriptions<SignatureSchemes>().ToArray();
        public static string[] VALID_PUB_EXP_MODES = EnumHelpers.GetEnumDescriptions<PublicExponentModes>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            parameters.Fips186Standard = Fips186Standard.Fips186_2;

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

                result = ValidateValue(capability.SigType, VALID_SIG_VER_MODES, "SigVer Modes");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    if (errorResults.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(moduloCap.HashPairs, "hash/salt pairs for modulus")))
                    {
                        continue;
                    }

                    result = ValidateValue(moduloCap.Modulo, VALID_MODULI, "Modulo");
                    if (!string.IsNullOrEmpty(result))
                    {
                        errorResults.Add(result);
                    }

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
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
                        // Check range for E value, larger range than normal SigVer
                        var eValue = new BitString(parameters.FixedPubExpValue).ToPositiveBigInteger();
                        if (eValue < 3 || eValue > (BigInteger)2 << 255 || eValue.IsEven)
                        {
                            errorResults.Add("Improper E value provided");
                        }
                    }
                }
            }

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

            // Special case for SHA-512 and 1024-bit RSA
            if (modulo == 1024 && maxSaltLen == 64)
            {
                maxSaltLen = 62;
            }

            if (saltLen > maxSaltLen)
            {
                return $"Salt Length must be below max value of {maxSaltLen} for given parameters";
            }

            return "";
        }
    }
}
