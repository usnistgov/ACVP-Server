using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization.Internal;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public const string Algorithm = "KAS-FFC";

        public static readonly string[] ValidFunctions = new string[]
        {
            "dpgen",
            "dpval",
            "keypairgen",
            "fullval",
            "keyregen"
        };
        public static readonly string[] ValidKeyAgreementRoles = new string[]
        {
            "initiator",
            "responder"
        };
        public static readonly string[] ValidHashAlgs = new string[]
        {
            "sha2-224",
            "sha2-256",
            "sha2-384",
            "sha2-512"
        };
        public static readonly int[] ValidAesKeyLengths = new int[] { 128, 192, 256 };
        public static readonly int[] ValidAesCcmNonceLengths = new int[] { 56, 64, 72, 80, 88, 96 };
        public static readonly string[] ValidKeyConfirmationRoles = new string[] { "provider", "recipient" };
        public static readonly string[] ValidKeyConfirmationTypes = new string[] { "unilateral", "bilateral" };
        public static readonly string[] ValidNonceTypes = new string[]
        {
            "randomNonce",
            "timestamp",
            "sequence",
            "timestampSequence"
        };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            // Validate Algorithm
            ValidateAlgorithm(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            // Validate at least one "function"
            ValidateFunction(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            // Validate Schemes
            ValidateSchemes(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateValue(parameters.Algorithm, new string[] {Algorithm}, "Algorithm"));
        }

        private void ValidateFunction(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Function, ValidFunctions, "Functions"));
        }

        private void ValidateSchemes(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Scheme == null)
            {
                errorResults.Add("Scheme is required.");
                return;
            }

            ValidateAtLeastOneSchemePresent(parameters.Scheme, errorResults);
            ValidateDhEphemScheme(parameters.Scheme.DhEphem, errorResults);
            ValidateMqv1Scheme(parameters.Scheme.Mqv1, errorResults);
        }

        #region scheme validation
        private void ValidateAtLeastOneSchemePresent(Schemes parametersScheme, List<string> errorResults)
        {
            // TODO add more schemes
            if (parametersScheme.DhEphem == null && parametersScheme.Mqv1 == null)
            {
                errorResults.Add("No schemes are present in the registration.");
            }
        }

        private void ValidateDhEphemScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.Role, errorResults);

            ValidateAtLeastOneKasModePresent(scheme, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            // kdfKc is invalid for dhEphem
            if (scheme.KdfKc != null && scheme is DhEphem)
            {
                errorResults.Add("Key Confirmation not possible with dhEphem.");
                return;
            }

            ValidateNoKdfNoKc(scheme.NoKdfNoKc, errorResults);
            ValidateKdfNoKc(scheme.KdfNoKc, errorResults);
            ValidateKdfKc(scheme.KdfKc, errorResults);
        }

        private void ValidateMqv1Scheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.Role, errorResults);

            ValidateAtLeastOneKasModePresent(scheme, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            ValidateNoKdfNoKc(scheme.NoKdfNoKc, errorResults);
            ValidateKdfNoKc(scheme.KdfNoKc, errorResults);
            ValidateKdfKc(scheme.KdfKc, errorResults);
        }

        private void ValidateKeyAgreementRoles(string[] schemeRoles, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }
        #endregion scheme validation

        #region kasModes
        private void ValidateAtLeastOneKasModePresent(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme.NoKdfNoKc == null && scheme.KdfNoKc == null && scheme.KdfKc == null)
            {
                errorResults.Add("At least one KAS mode must be provided.");
            }
        }

        private void ValidateNoKdfNoKc(NoKdfNoKc kasMode, List<string> errorResults)
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

            ValidateParameterSetFfc(kasMode.ParameterSet.Fb, false, FfcParameterSet.Fb, errorResults);
            ValidateParameterSetFfc(kasMode.ParameterSet.Fc, false, FfcParameterSet.Fc, errorResults);
        }

        private void ValidateKdfNoKc(KdfNoKc kasMode, List<string> errorResults)
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

            ValidateParameterSetFfc(kasMode.ParameterSet.Fb, true, FfcParameterSet.Fb, errorResults);
            ValidateParameterSetFfc(kasMode.ParameterSet.Fc, true, FfcParameterSet.Fc, errorResults);
            ValidateKdfOption(kasMode.KdfOption, errorResults);
        }

        private void ValidateKdfKc(KdfKc kasMode, List<string> errorResults)
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

            ValidateParameterSetFfc(kasMode.ParameterSet.Fb, true, FfcParameterSet.Fb, errorResults);
            ValidateParameterSetFfc(kasMode.ParameterSet.Fc, true, FfcParameterSet.Fc, errorResults);
            ValidateKdfOption(kasMode.KdfOption, errorResults);
            ValidateKcOption(kasMode.KcOption, errorResults);
        }
        #endregion kasModes

        #region parameterSet
        private void ValidateAtLeastOneParameterSetPresent(NoKdfNoKc kasMode, List<string> errorResults)
        {
            if (kasMode.ParameterSet == null)
            {
                errorResults.Add("ParameterSet must be provided.");
                return;
            }

            if (kasMode.ParameterSet.Fb == null && kasMode.ParameterSet.Fc == null)
            {
                errorResults.Add("At least one paramter set must be provided.");
            }
        }

        private void ValidateParameterSetFfc(ParameterSetBase parameterSet, bool macRequired, FfcParameterSet parameterSetType, List<string> errorResults)
        {
            if (parameterSet == null)
            {
                return;
            }

            var fbDetails = FfcParameterSetDetails.GetDetailsForParameterSet(parameterSetType);

            ValidateHashFunctions(parameterSet.HashAlg, parameterSetType, fbDetails.minHashLength, errorResults);
            ValidateMacOptions(parameterSet.MacOption, macRequired, parameterSetType, fbDetails.minMacLength,
                fbDetails.minMacKeyLength, errorResults);
        }

        private void ValidateHashFunctions(string[] hashAlg, FfcParameterSet parameterSetType, int minHashLength, List<string> errorResults)
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

        private void ValidateMacOptions(MacOptions macOption, bool macRequired, FfcParameterSet parameterSetType, int minMacLength, int minMacKeyLength, List<string> errorResults)
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
                    macOption.HmacSha2_D512 == null)
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
            }
            else
            {
                if (macOption != null)
                {
                    errorResults.Add($"{nameof(macOption)} not expected for current KAS options.");
                }
            }
        }

        private void ValidateMacOptions(MacOptionsBase macOptionsBase, int minMacLength, int minMacKeyLength, List<string> errorResults)
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
                errorResults.AddIfNotNullOrEmpty(ValidateArray(macOptionsBase.KeyLen, ValidAesKeyLengths, "AES Key Lengths"));
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
            if (macOptionsBase.KeyLen.Min() < minMacKeyLength)
            {
                errorResults.Add("KeyLength minimum");
            }
            // validate mac mod 8
            errorResults.AddIfNotNullOrEmpty(ValidateMultipleOf(new int[] {macOptionsBase.MacLen}, 8,
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

            const string oiRegex = @"^((?!(uPartyInfo|vPartyInfo|counter|literal\[[0-9a-fA-F]+\])).)+$";

            var oiPieces = oiPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var oiPiece in oiPieces)
            {
                Regex regex = new Regex(oiRegex, RegexOptions.IgnoreCase);
                if (regex.IsMatch(oiPiece))
                {
                    errorResults.Add($"{nameof(oiPattern)} has invalid element {oiPiece}");
                }
            }
        }
        #endregion KDF

        #region KC
        private void ValidateKcOption(KcOptions kasModeKcOption, List<string> errorResults)
        {
            if (kasModeKcOption == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.KcRole, ValidKeyConfirmationRoles, "Key Confirmation Roles"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.KcType, ValidKeyConfirmationTypes, "Key Confirmation Types"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(kasModeKcOption.NonceType, ValidNonceTypes, "Key Confirmation Nonce Types"));
        }
        #endregion KC
    }
}
