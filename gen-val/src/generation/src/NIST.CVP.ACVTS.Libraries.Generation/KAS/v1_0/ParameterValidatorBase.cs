using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0
{
    public abstract class ParameterValidatorBase : Core.ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public abstract string Algorithm { get; }
        public abstract string Mode { get; }

        public abstract string[] ValidFunctions { get; }

        public static readonly string[] ValidKeyAgreementRoles = new string[]
        {
            "initiator",
            "responder"
        };
        public static readonly string[] ValidHashAlgs = new string[]
        {
            "SHA-1",
            "SHA2-224",
            "SHA2-256",
            "SHA2-384",
            "SHA2-512"
        };
        public static readonly int[] ValidAesKeyLengths = new int[] { 128, 192, 256 };
        public static readonly int[] ValidAesCcmNonceLengths = new int[] { 56, 64, 72, 80, 88, 96, 104 };
        public static readonly string[] ValidKeyConfirmationRoles = new string[] { "provider", "recipient" };
        public static readonly string[] ValidKeyConfirmationTypes = new string[] { "unilateral", "bilateral" };
        public static readonly string[] ValidNonceTypes = new string[]
        {
            "randomNonce",
            "timestamp",
            "sequence",
            "timestampSequence"
        };
        private static readonly string[] ValidFixedInfoPatternPieces =
        {
            "uPartyInfo",
            "vPartyInfo",
            "counter"
        };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            // Validate Algorithm
            ValidateAlgorithm(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            // Validate at least one "function"
            ValidateFunction(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            // Validate Schemes
            ValidateSchemes(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateValue(parameters.Algorithm, new string[] { Algorithm }, "Algorithm"));
        }

        private void ValidateFunction(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Function == null || parameters.Function.Length == 0)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Function, ValidFunctions, "Functions"));
        }

        protected abstract void ValidateSchemes(Parameters parameters, List<string> errorResults);


        #region scheme validation
        protected void ValidateKeyAgreementRoles(string[] schemeRoles, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }
        #endregion scheme validation

        #region kasModes
        protected void ValidateAtLeastOneKasModePresent(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme.NoKdfNoKc == null && scheme.KdfNoKc == null && scheme.KdfKc == null)
            {
                errorResults.Add("At least one KAS mode must be provided.");
            }
        }

        protected void ValidateNoKdfNoKc(NoKdfNoKc kasMode, List<string> errorResults)
        {
            if (kasMode == null)
            {
                return;
            }

            ValidateAtLeastOneParameterSetPresent(kasMode, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            ValidateParameterSets(kasMode, false, errorResults);
        }

        protected abstract void ValidateParameterSets(NoKdfNoKc kasMode, bool macRequired, List<string> errorResults);

        protected void ValidateKdfNoKc(KdfNoKc kasMode, List<string> errorResults)
        {
            if (kasMode == null)
            {
                return;
            }

            ValidateAtLeastOneParameterSetPresent(kasMode, errorResults);
            if (kasMode.KdfOption == null)
            {
                errorResults.Add($"{nameof(kasMode.KdfOption)} are required.");
            }

            if (errorResults.Count > 0)
            {
                return;
            }

            ValidateParameterSets(kasMode, true, errorResults);
            ValidateKdfOption(kasMode.KdfOption, errorResults);
        }

        protected void ValidateKdfKc(KdfKc kasMode, List<string> errorResults)
        {
            if (kasMode == null)
            {
                return;
            }

            ValidateAtLeastOneParameterSetPresent(kasMode, errorResults);
            if (kasMode.KdfOption == null)
            {
                errorResults.Add($"{nameof(kasMode.KdfOption)} are required.");
            }

            if (errorResults.Count > 0)
            {
                return;
            }

            ValidateParameterSets(kasMode, true, errorResults);
            ValidateKdfOption(kasMode.KdfOption, errorResults);
            ValidateKcOption(kasMode.KcOption, errorResults);
        }
        #endregion kasModes

        #region parameterSet

        protected abstract void ValidateAtLeastOneParameterSetPresent(NoKdfNoKc kasMode, List<string> errorResults);

        protected void ValidateHashFunctions(string[] hashAlg, string parameterSetType, int minHashLength, List<string> errorResults)
        {
            // Validate valid entries
            errorResults.AddIfNotNullOrEmpty(ValidateArray(hashAlg, ValidHashAlgs, "HashAlgs"));
            if (errorResults.Count > 0)
            {
                return;
            }

            // Validate minimums
            foreach (var hash in hashAlg)
            {
                var hashAttributes = ShaAttributes.GetShaAttributes(hash);

                if (hashAttributes.outputLen < minHashLength)
                {
                    errorResults.Add($"Specified {nameof(hash)} length of {hash} is not valid for {nameof(parameterSetType)} {parameterSetType}");
                }
            }
        }

        protected void ValidateMacOptions(MacOptions macOption, bool macRequired, int minMacLength, int minMacKeyLength, List<string> errorResults)
        {
            if (macRequired)
            {
                if (macOption == null)
                {
                    errorResults.Add($"{nameof(macOption)} required for current KAS options.");
                    return;
                }

                if (macOption.AesCcm == null && macOption.Cmac == null && macOption.HmacSha2_D224 == null &&
                    macOption.HmacSha2_D256 == null && macOption.HmacSha2_D384 == null &&
                    macOption.HmacSha2_D512 == null && macOption.HmacSha2_D512_T224 == null && macOption.HmacSha2_D512_T256 == null &&
                    macOption.HmacSha3_D224 == null && macOption.HmacSha3_D256 == null && macOption.HmacSha3_D384 == null && macOption.HmacSha3_D512 == null)
                {
                    errorResults.Add($"At least one valid {nameof(macOption)} is required");
                    return;
                }

                ValidateMacOptions(macOption.AesCcm, minMacLength, minMacKeyLength, errorResults);

                ValidateMacOptions(macOption.Cmac, minMacLength, minMacKeyLength, errorResults);

                ValidateMacOptions(macOption.HmacSha2_D224, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha2_D256, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha2_D384, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha2_D512, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha2_D512_T224, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha2_D512_T256, minMacLength, minMacKeyLength, errorResults);

                ValidateMacOptions(macOption.HmacSha3_D224, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha3_D256, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha3_D384, minMacLength, minMacKeyLength, errorResults);
                ValidateMacOptions(macOption.HmacSha3_D512, minMacLength, minMacKeyLength, errorResults);
            }
            else
            {
                if (macOption != null)
                {
                    errorResults.Add($"{nameof(macOption)} not expected for current KAS options.");
                }
            }
        }

        protected void ValidateMacOptions(MacOptionsBase macOptionsBase, int minMacLength, int minMacKeyLength, List<string> errorResults)
        {
            if (macOptionsBase == null)
            {
                return;
            }

            if (macOptionsBase is MacOptionAesCcm)
            {
                errorResults.AddIfNotNullOrEmpty(ValidateValue(macOptionsBase.NonceLen, ValidAesCcmNonceLengths, "AES-CCM Nonce Length"));
            }
            else
            {
                if (macOptionsBase.NonceLen != 0)
                {
                    errorResults.Add($"{macOptionsBase.NonceLen} unexpected for macOptions");
                }
            }

            int maxMacLength;
            if (macOptionsBase is MacOptionAesCcm || macOptionsBase is MacOptionCmac)
            {
                var copyKeyLen = macOptionsBase.KeyLen.GetDeepCopy();
                var keyLenValues = copyKeyLen.GetValues(3).ToArray(); // maximum of 3 valid key values with AES

                errorResults.AddIfNotNullOrEmpty(ValidateArray(keyLenValues, ValidAesKeyLengths, "AES Key Lengths"));
                maxMacLength = 128; // aes block size
            }
            else
            {
                var macAttributes = SpecificationMapping.GetHmacInfoFromParameterClass(macOptionsBase);
                maxMacLength = macAttributes.hashFunction.OutputLen;
            }

            // validate key mod 8
            errorResults.AddIfNotNullOrEmpty(ValidateMultipleOf(macOptionsBase.KeyLen, 8, "KeyLength Modulus"));
            // valid key min
            if (macOptionsBase.KeyLen.GetDomainMinMax().Minimum < minMacKeyLength)
            {
                errorResults.Add("KeyLength minimum");
            }
            // validate mac mod 8
            errorResults.AddIfNotNullOrEmpty(ValidateMultipleOf(new int[] { macOptionsBase.MacLen }, 8,
                "MacLength Modulus"));
            // validate mac length meets min/max requirements
            if (macOptionsBase.MacLen < minMacLength ||
                macOptionsBase.MacLen > maxMacLength)
            {
                errorResults.Add("MacLength Range");
            }
        }
        #endregion paramterSet

        #region KDF
        private void ValidateKdfOption(KdfOptions kasModeKdfOption, List<string> errorResults)
        {
            ValidateAtLeastOneKdfProvided(kasModeKdfOption, errorResults);
            ValidateKdfNoKc(kasModeKdfOption.Asn1, errorResults);
            ValidateKdfNoKc(kasModeKdfOption.Concatenation, errorResults);
        }

        private void ValidateAtLeastOneKdfProvided(KdfOptions kasModeKdfOption, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(kasModeKdfOption.Asn1) && string.IsNullOrEmpty(kasModeKdfOption.Concatenation))
            {
                errorResults.Add($"At least one {nameof(kasModeKdfOption)} is required.");
            }
        }

        private void ValidateKdfNoKc(string oiPattern, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(oiPattern))
            {
                return;
            }

            Regex notHexRegex = new Regex(@"[^0-9a-fA-F]", RegexOptions.IgnoreCase);
            string literalStart = "literal[";
            string literalEnd = "]";

            var fiPieces = oiPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (fiPieces?.Length == 0)
            {
                errorResults.Add($"Invalid {nameof(oiPattern)} {oiPattern}");
            }

            var allUniquePieces = fiPieces
                .GroupBy(gb => gb)
                .All(a => a.Count() == 1);

            if (!allUniquePieces)
            {
                errorResults.Add($"Duplicate pieces of {nameof(oiPattern)} found; pieces should be unique.");
            }

            foreach (var fiPiece in fiPieces)
            {
                if (fiPiece.StartsWith(literalStart) && fiPiece.EndsWith(literalEnd))
                {
                    var tempLiteral = fiPiece.Replace(literalStart, string.Empty);
                    tempLiteral = tempLiteral.Replace(literalEnd, string.Empty);

                    if (notHexRegex.IsMatch(tempLiteral))
                    {
                        errorResults.Add("literal element of oiPattern contained non hex values.");
                        continue;
                    }

                    try
                    {
                        _ = new BitString(tempLiteral);
                    }
                    catch (InvalidBitStringLengthException e)
                    {
                        errorResults.Add(e.Message);
                    }

                    continue;
                }

                if (!ValidFixedInfoPatternPieces.Contains(fiPiece))
                {
                    errorResults.Add($"Invalid portion of oiPattern: {fiPiece}");
                }
            }
        }
        #endregion KDF

        #region KC
        private void ValidateKcOption(KcOptions kasModeKcOption, List<string> errorResults)
        {
            if (kasModeKcOption == null)
            {
                errorResults.Add("KcOption is required when for key confirmation registrations");
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.KcRole, ValidKeyConfirmationRoles, "Key Confirmation Roles"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.KcType, ValidKeyConfirmationTypes, "Key Confirmation Types"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.NonceType, ValidNonceTypes, "Key Confirmation Nonce Types"));
        }
        #endregion KC
    }
}
