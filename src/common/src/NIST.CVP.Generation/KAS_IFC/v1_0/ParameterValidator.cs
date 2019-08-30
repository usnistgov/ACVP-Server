using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        
        #region Validation statics
        public static readonly KeyAgreementRole[] ValidKeyAgreementRoles =
        {
            KeyAgreementRole.InitiatorPartyU,
            KeyAgreementRole.ResponderPartyV
        };
        public static readonly KasHashAlg[] ValidHashAlgs =
        {
            KasHashAlg.SHA2_D224,
            KasHashAlg.SHA2_D256,
            KasHashAlg.SHA2_D384,
            KasHashAlg.SHA2_D512,
            KasHashAlg.SHA2_D512_T224,
            KasHashAlg.SHA2_D512_T256,
            KasHashAlg.SHA3_D224,
            KasHashAlg.SHA3_D256,
            KasHashAlg.SHA3_D384,
            KasHashAlg.SHA3_D512
        };

        public static readonly (KasKdfOneStepAuxFunction functionName, bool requiresSaltLength)[] ValidAuxMethods = 
        {
            (KasKdfOneStepAuxFunction.SHA2_D224, false),
            (KasKdfOneStepAuxFunction.SHA2_D256, false),
            (KasKdfOneStepAuxFunction.SHA2_D384, false),
            (KasKdfOneStepAuxFunction.SHA2_D512, false),
            (KasKdfOneStepAuxFunction.SHA2_D512_T224, false),
            (KasKdfOneStepAuxFunction.SHA2_D512_T256, false),
            (KasKdfOneStepAuxFunction.SHA3_D224, false),
            (KasKdfOneStepAuxFunction.SHA3_D256, false),
            (KasKdfOneStepAuxFunction.SHA3_D384, false),
            (KasKdfOneStepAuxFunction.SHA3_D512, false),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D224, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D256, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D384, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D512, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA3_D224, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA3_D256, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA3_D384, true),
            (KasKdfOneStepAuxFunction.HMAC_SHA3_D512, true)
        };

        private static readonly AlgoMode[] ValidAlgoModes = new[]
        {
            AlgoMode.KAS_IFC_v1_0,
            AlgoMode.KTS_IFC_v1_0
        };

        private static readonly IfcScheme[] ValidKasSchemes = new[]
        {
            IfcScheme.Kas1_basic,
            IfcScheme.Kas1_partyV_keyConfirmation,
            IfcScheme.Kas2_basic,
            IfcScheme.Kas2_bilateral_keyConfirmation,
            IfcScheme.Kas2_partyU_keyConfirmation,
            IfcScheme.Kas2_partyV_keyConfirmation
        };
        private static readonly IfcScheme[] ValidKtsSchemes = new[]
        {
            IfcScheme.Kts_oaep_basic,
            IfcScheme.Kts_oaep_partyV_keyConfirmation,
        };
        private static readonly IfcScheme[] SchemesRequiringKeyConfirmation = new[]
        {
            IfcScheme.Kas1_partyV_keyConfirmation,
            IfcScheme.Kas2_bilateral_keyConfirmation,
            IfcScheme.Kas2_partyU_keyConfirmation,
            IfcScheme.Kas2_partyV_keyConfirmation,
            IfcScheme.Kts_oaep_partyV_keyConfirmation
        };

        private static readonly IfcKeyGenerationMethod[] ValidKeyGenerationMethods = new[]
        {
            IfcKeyGenerationMethod.RsaKpg1_basic,
            IfcKeyGenerationMethod.RsaKpg1_primeFactor,
            IfcKeyGenerationMethod.RsaKpg1_crt,
            IfcKeyGenerationMethod.RsaKpg2_basic,
            IfcKeyGenerationMethod.RsaKpg2_primeFactor,
            IfcKeyGenerationMethod.RsaKpg2_crt,
        };

        private static readonly int[] ValidModulo = ParameterSetDetails.RsaModuloDetails.Keys.ToArray();

        private static readonly int[] ValidAesKeyLengths = new[] {128, 192, 256};
        private static readonly int MinimumL = 112;
        private static readonly int MaximumL = 1024;
        #endregion Validation statics

        private AlgoMode _algoMode;
        private bool _isKasScheme;
        private bool _isKtsScheme;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();
            _algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            if (_algoMode == AlgoMode.KAS_IFC_v1_0)
            {
                _isKasScheme = true;
            }
            
            if (_algoMode == AlgoMode.KTS_IFC_v1_0)
            {
                _isKtsScheme = true;
            }
            
            if (!ValidAlgoModes.Contains(_algoMode))
            {
                errorResults.Add("Invalid Algorithm, mode, revision combination.");
                return new ParameterValidateResponse(errorResults);
            }

            ValidateSchemes(parameters, errorResults);
            
            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateSchemes(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Scheme == null)
            {
                errorResults.Add("Scheme is required.");
                return;
            }

            var registeredSchemes = parameters.Scheme.GetRegisteredSchemes();
            
            if (!registeredSchemes.Any())
            {
                errorResults.Add("No Schemes were registered");
                return;
            }
            
            ValidateSchemesForAlgoMode(errorResults, registeredSchemes);

            if (errorResults.Any())
            {
                return;
            }
            
            ValidateScheme(parameters.Scheme.Kas1_basic, errorResults);
            ValidateScheme(parameters.Scheme.Kas1_partyV_confirmation, errorResults);
            ValidateScheme(parameters.Scheme.Kas2_basic, errorResults);
            ValidateScheme(parameters.Scheme.Kas2_bilateral_confirmation, errorResults);
            ValidateScheme(parameters.Scheme.Kas2_partyU_confirmation, errorResults);
            ValidateScheme(parameters.Scheme.Kas2_partyV_confirmation, errorResults);
            ValidateScheme(parameters.Scheme.Kts_oaep_basic, errorResults);
            ValidateScheme(parameters.Scheme.Kts_oaep_partyV_confirmation, errorResults);
        }
       
        private void ValidateSchemesForAlgoMode(List<string> errorResults, IEnumerable<SchemeBase> registeredSchemes)
        {
            var invalidSchemeMessage = $"Invalid Schemes for registered {_algoMode}";
            
            switch (_algoMode)
            {
                case AlgoMode.KAS_IFC_v1_0:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(ValidKtsSchemes).Any())
                    {
                        errorResults.Add(invalidSchemeMessage);
                    }
                    break;
                case AlgoMode.KTS_IFC_v1_0:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(ValidKasSchemes).Any())
                    {
                        errorResults.Add(invalidSchemeMessage);
                    }
                    break;
                default:
                    throw new Exception($"Invalid {_algoMode}");
            }
        }
        
        private void ValidateScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            if (scheme.L < MinimumL || scheme.L > MaximumL)
            {
                errorResults.Add($"{nameof(scheme.L)} should be within the range of {MinimumL} and {MaximumL}");
            }
            
            ValidateKeyAgreementRoles(scheme.KasRole, errorResults);
            ValidateKeyGenerationMethod(scheme.KeyGenerationMethods, errorResults);
            ValidateKdfMethods(scheme.KdfMethods, errorResults);
            ValidateKtsMethods(scheme.KtsMethod, errorResults);
            ValidateMacMethods(scheme.Scheme, scheme.MacMethods, scheme.L, errorResults);
        }

        private void ValidateKeyAgreementRoles(KeyAgreementRole[] schemeRoles, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }
        
        private void ValidateKeyGenerationMethod(KeyGenerationMethods schemeKeyGenerationMethods, List<string> errorResults)
        {
            if (schemeKeyGenerationMethods == null)
            {
                errorResults.Add($"{nameof(schemeKeyGenerationMethods)} not provided.");
                return;
            }

            var keyGenerationMethods = schemeKeyGenerationMethods.GetRegisteredKeyGenerationMethods();
            
            errorResults.AddIfNotNullOrEmpty(ValidateArray(
                keyGenerationMethods.Select(s => s.KeyGenerationMethod), ValidKeyGenerationMethods, "Key Agreement Roles"));
            
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg1_basic, errorResults, true);
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg1_primeFactor, errorResults, true);
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg1_crt, errorResults, true);
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg2_basic, errorResults, false);
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg2_primeFactor, errorResults, false);
            ValidateKeyGenerationMethod(schemeKeyGenerationMethods.RsaKpg2_crt, errorResults, false);
        }

        private void ValidateKeyGenerationMethod(KeyGenerationMethodBase keyGenBase, List<string> errorResults, bool requiresFixedPublicKey)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(keyGenBase.Modulo, ValidModulo, "Modulus"));

            if (requiresFixedPublicKey && keyGenBase.FixedPublicExponent?.BitLength == 0)
            {
                errorResults.Add("Fixed public exponent required for this method of key generation");
            }

            if (!requiresFixedPublicKey && keyGenBase.FixedPublicExponent?.BitLength != 0)
            {
                errorResults.Add("Unexpected fixed public exponent");
            }
        }
        
        #region kdfValidation
        private void ValidateKdfMethods(KdfMethods schemeKdfMethods, List<string> errorResults)
        {
            // KTS Schemes don't use key derivation.
            if (_isKtsScheme)
            {
                return;
            }

            if (schemeKdfMethods == null)
            {
                errorResults.Add("A KDF is required and was not provided.");
                return;
            }

            var registeredKdfs = schemeKdfMethods.GetRegisteredKdfMethods();
            
            if (!registeredKdfs.Any())
            {
                return;
            }
            
            ValidateKdfMethod(schemeKdfMethods.OneStepKdf, errorResults);
        }

        #region OneStepKdf
        private void ValidateKdfMethod(OneStepKdf oneStepKdf, List<string> errorResults)
        {
            if (oneStepKdf == null)
            {
                return;
            }

            ValidateAuxFunction(oneStepKdf.AuxFunctions, errorResults);
            ValidateEncoding(oneStepKdf.Encoding, errorResults);
        }

        private void ValidateAuxFunction(AuxFunction[] auxFunctions, List<string> errorResults)
        {
            if (auxFunctions == null || !auxFunctions.Any())
            {
                errorResults.Add("at least one AuxFunction is required.");
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(
                auxFunctions.Select(s => s.AuxFunctionName), 
                ValidAuxMethods.Select(s => s.functionName), 
                $"Aux Function"));

            var validSaltGenerationMethods = new MacSaltMethod[] {MacSaltMethod.Default, MacSaltMethod.Random};
            
            foreach (var auxFunction in auxFunctions)
            {
                var needsSalt = ValidAuxMethods.First(w =>
                        w.functionName.Equals(auxFunction.AuxFunctionName))
                    .requiresSaltLength;

                if (needsSalt)
                {
                    if (auxFunction.SaltLen == 0)
                    {
                        errorResults.Add($"Expected salt length for {nameof(auxFunction)} {auxFunction.AuxFunctionName}");
                    }
                    
                    errorResults.AddIfNotNullOrEmpty(ValidateArray(auxFunction.MacSaltMethods, validSaltGenerationMethods, nameof(MacSaltMethod)));

                }

                if (!needsSalt)
                {
                    if (auxFunction.SaltLen != 0)
                    {
                        errorResults.Add($"Unexpected salt length for {nameof(auxFunction)} {auxFunction.AuxFunctionName}");
                    }

                    errorResults.AddIfNotNullOrEmpty(ValidateArray(auxFunction.MacSaltMethods, new MacSaltMethod[] { MacSaltMethod.None }, nameof(MacSaltMethod)));
                }
                
                ValidateFixedInputPattern(auxFunction.FixedInputPattern, errorResults);
            }
        }

        private void ValidateFixedInputPattern(string auxFunctionFixedInputPattern, List<string> errorResults)
        {
            const string fiRegex = @"^((?!(salt|uPartyInfo|vPartyInfo|counter|literal\[[0-9a-fA-F]+\])).)+$";

            var fiPieces = auxFunctionFixedInputPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var fiPiece in fiPieces)
            {
                Regex regex = new Regex(fiRegex, RegexOptions.IgnoreCase);
                if (regex.IsMatch(fiPiece))
                {
                    errorResults.Add($"{nameof(auxFunctionFixedInputPattern)} has invalid element {fiPiece}");
                }
            }
        }

        private void ValidateEncoding(KasKdfFixedInfoEncoding[] encoding, List<string> errorResults)
        {
            var validEncodingTypes = new[] { KasKdfFixedInfoEncoding.Concatenation, KasKdfFixedInfoEncoding.ASN_1 };

            errorResults.AddIfNotNullOrEmpty(ValidateArray(encoding, validEncodingTypes, "One Step KDF encoding type"));
        }
        #endregion OneStepKdf
        #endregion kdfValidation
        
        #region ktsValidation
        private void ValidateKtsMethods(KtsMethod schemeKtsCapabilities, List<string> errorResults)
        {
            // Kas schemes don't use KTS methods
            if (_isKasScheme)
            {
                return;
            }
            
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeKtsCapabilities.HashAlgs, ValidHashAlgs, "KTS HashAlgs"));
            ValidateAssociatedDataPattern(schemeKtsCapabilities.AssociatedDataPattern, errorResults);
        }

        private void ValidateAssociatedDataPattern(string associatedDataPattern, List<string> errorResults)
        {
            const string fiRegex = @"^((?!(salt|uPartyInfo|vPartyInfo|counter|literal\[[0-9a-fA-F]+\])).)+$";

            var fiPieces = associatedDataPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var fiPiece in fiPieces)
            {
                Regex regex = new Regex(fiRegex, RegexOptions.IgnoreCase);
                if (regex.IsMatch(fiPiece))
                {
                    errorResults.Add($"{nameof(associatedDataPattern)} has invalid element {fiPiece}");
                }
            }
        }
        #endregion ktsValidation
        
        #region macValidation
        private void ValidateMacMethods(IfcScheme scheme, MacMethods macOptions, int l, List<string> errorResults)
        {
            var schemeRequiresMac = SchemesRequiringKeyConfirmation.Contains(scheme);

            if (!schemeRequiresMac)
            {
                return;
            }

            if (macOptions == null)
            {
                errorResults.Add($"{nameof(macOptions)} was null.");
                return;
            }

            if (!macOptions.GetRegisteredMacMethods().Any())
            {
                errorResults.Add($"{nameof(macOptions)} contained no registered MAC algorithms.");
            }
            
            ValidateMacKeyLen(l, macOptions.Cmac, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D224, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D256, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D384, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D512, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D512_T224, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha2_D512_T256, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha3_D224, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha3_D256, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha3_D384, errorResults);
            ValidateMacKeyLen(l, macOptions.HmacSha3_D512, errorResults);
        }

        private void ValidateMacKeyLen(int schemeL, MacOptionsBase macMethod, List<string> errorResults)
        {
            var keyConfirmationMacDetails =
                KeyGenerationRequirementsHelper.GetKeyConfirmationMacDetails(macMethod.MacType);

            if (schemeL < macMethod.KeyLen)
            {
                errorResults.Add($"Provided {nameof(schemeL)} value does not contain enough keying material to perform key confirmation with {macMethod.MacType}");
            }
            if (macMethod.KeyLen < keyConfirmationMacDetails.MinKeyLen || macMethod.KeyLen > keyConfirmationMacDetails.MaxKeyLen || 
                (macMethod.MacType == KeyAgreementMacType.CmacAes && !ValidAesKeyLengths.Contains(macMethod.KeyLen)))
            {
                errorResults.Add($"The provided {nameof(macMethod.KeyLen)} is outside the bounds of acceptable values for the specified {nameof(macMethod)} {macMethod.MacType}");
            }

            if (macMethod.MacLen < keyConfirmationMacDetails.MinTagLen ||
                macMethod.MacLen > keyConfirmationMacDetails.MaxTagLen)
            {
                errorResults.Add($"The provided {nameof(macMethod.MacLen)} is outside the bounds of acceptable values for the specified {nameof(macMethod)} {macMethod.MacType}");
            }
        }
        #endregion macValidation
    }
}