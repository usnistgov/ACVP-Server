using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public string[] ValidFunctions => new string[]
        {
            "keyPairGen",
            "partialVal",
        };

        // TODO remove this enum and field, use HashFunctions enum instead
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

        public static readonly HashFunctions[] ValidHashFunctions =
        {
            HashFunctions.Sha1,
            HashFunctions.Sha2_d224,
            HashFunctions.Sha2_d256,
            HashFunctions.Sha2_d384,
            HashFunctions.Sha2_d512,
            HashFunctions.Sha2_d512t224,
            HashFunctions.Sha2_d512t256,
            HashFunctions.Sha3_d224,
            HashFunctions.Sha3_d256,
            HashFunctions.Sha3_d384,
            HashFunctions.Sha3_d512,
        };

        public static readonly HashFunctions[] ValidHashFunctionsTlsV12 =
        {
            HashFunctions.Sha2_d256,
            HashFunctions.Sha2_d384,
            HashFunctions.Sha2_d512,
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
            (KasKdfOneStepAuxFunction.HMAC_SHA3_D512, true),
            (KasKdfOneStepAuxFunction.KMAC_128, true),
            (KasKdfOneStepAuxFunction.KMAC_256, true),
        };

        private static readonly AlgoMode[] ValidAlgoModes = new[]
        {
            AlgoMode.KAS_IFC_v1_0,
            AlgoMode.KTS_IFC_v1_0
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

        private static readonly MacSaltMethod[] ValidSaltGenerationMethods = { MacSaltMethod.Default, MacSaltMethod.Random };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes = new[]
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };

        private static readonly int[] ValidModulo = ParameterSetDetails.RsaModuloDetails.Keys.ToArray();

        private static readonly int[] ValidAesKeyLengths = new[] { 128, 192, 256 };
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

            ValidateFunction(parameters, errorResults);
            ValidateSchemes(parameters, errorResults);
            ValidateKeys(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunction(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Function == null || parameters.Function.Length == 0)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Function, ValidFunctions, "Functions"));
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
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(KeyGenerationRequirementsHelper.IfcKtsSchemes).Any())
                    {
                        errorResults.Add(invalidSchemeMessage);
                    }
                    break;
                case AlgoMode.KTS_IFC_v1_0:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(KeyGenerationRequirementsHelper.IfcKdfSchemes).Any())
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

            if (scheme.L % BitString.BITSINBYTE != 0)
            {
                errorResults.Add($"{nameof(scheme.L)} mod 8 should equal 0.");
            }

            ValidateKeyAgreementRoles(scheme.KasRole, errorResults);
            ValidateKeyGenerationMethod(scheme.KeyGenerationMethods, errorResults);
            ValidateKdfMethods(scheme.KdfMethods, scheme.L, errorResults);
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
            if (keyGenBase == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(keyGenBase.Modulo, ValidModulo, "Modulus"));

            if (requiresFixedPublicKey && !RsaKeyHelper.IsValidExponent(keyGenBase.FixedPublicExponent))
            {
                errorResults.Add("Valid fixed public exponent required for this method of key generation");
            }

            if (!requiresFixedPublicKey && keyGenBase.FixedPublicExponent != 0)
            {
                errorResults.Add("Unexpected fixed public exponent");
            }
        }

        #region kdfValidation
        private void ValidateKdfMethods(KdfMethods schemeKdfMethods, int l, List<string> errorResults)
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

            ValidateKdfMethod(schemeKdfMethods.OneStepKdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.TwoStepKdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.IkeV1Kdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.IkeV2Kdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.TlsV10_11Kdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.TlsV12Kdf, l, errorResults);
        }

        #region OneStepKdf
        private void ValidateKdfMethod(OneStepKdf kdf, int l, List<string> errorResults)
        {
            if (kdf == null)
            {
                return;
            }

            ValidateAuxFunction(kdf.AuxFunctions, errorResults);
            // perhaps do in a "base" kdf validator?
            ValidateFixedInfoPattern(kdf.FixedInfoPattern, errorResults, new List<string>() { "uPartyInfo", "vPartyInfo" });
            ValidateEncoding(kdf.Encoding, errorResults);
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

                    if (auxFunction.SaltLen > 512)
                    {
                        errorResults.Add("Salt length may not exceed 512.");
                    }

                    errorResults.AddIfNotNullOrEmpty(ValidateArray(auxFunction.MacSaltMethods, ValidSaltGenerationMethods, nameof(MacSaltMethod)));

                }

                if (!needsSalt)
                {
                    if (auxFunction.SaltLen != 0)
                    {
                        errorResults.Add($"Unexpected salt length for {nameof(auxFunction)} {auxFunction.AuxFunctionName}");
                    }

                    errorResults.AddIfNotNullOrEmpty(ValidateArray(auxFunction.MacSaltMethods, new[] { MacSaltMethod.None }, nameof(MacSaltMethod)));
                }
            }
        }

        #endregion OneStepKdf

        #region TwoStepKdf
        private void ValidateKdfMethod(TwoStepKdf kdf, int l, List<string> errorResults)
        {
            if (kdf == null)
            {
                return;
            }

            var kdfTwoStepValidator = new KDF.v1_0.ParameterValidator();
            var kdfParam = new KDF.v1_0.Parameters()
            {
                Algorithm = "KDF",
                Revision = "1.0",
                Capabilities = kdf.Capabilities
            };

            var validate = kdfTwoStepValidator.Validate(kdfParam);
            if (!validate.Success)
            {
                errorResults.AddRangeIfNotNullOrEmpty(validate.Errors);
                return;
            }

            foreach (var capability in kdf.Capabilities)
            {
                ValidateFixedInfoPattern(capability.FixedInfoPattern, errorResults, null);
                ValidateEncoding(capability.Encoding, errorResults);
                errorResults.AddIfNotNullOrEmpty(ValidateArray(capability.MacSaltMethods, ValidSaltGenerationMethods, nameof(MacSaltMethod)));

                // Ensure the L value is contained within the capabilities domain
                if (!capability.SupportedLengths.IsWithinDomain(l))
                {
                    errorResults.Add($"Provided {nameof(l)} value of {l} was not contained within the {nameof(capability.SupportedLengths)} domain.");
                }
            }
        }
        #endregion TwoStepKdf

        #region IkeV1
        private void ValidateKdfMethod(IkeV1Kdf kdf, int l, List<string> errorResults)
        {
            if (kdf == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kdf.HashFunctions, ValidHashFunctions, "IKEv1 HashFunctions"));

            // TODO this needs confirmation with the CT group as to the method of concatenation of the DKM with this KDF (if it exists at all).
            // We need to ensure that (if using concatenation) the desired L is at a minimum the length of the hash output * 3.
            var isValid = false;
            foreach (var hashAlg in kdf.HashFunctions)
            {
                var hashFunction = ShaAttributes.GetHashFunctionFromEnum(hashAlg);
                // TODO remove multiply by 3 if concatenation is not used.
                var outputLen = hashFunction.OutputLen * 3;

                if (l <= outputLen)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
            {
                errorResults.Add($"No provided {nameof(HashFunction)} in use for IkeV1 would provide enough keying material to meet {nameof(l)}.");
            }
        }
        #endregion IkeV1

        #region IkeV2
        private void ValidateKdfMethod(IkeV2Kdf kdf, int i, List<string> errorResults)
        {
            if (kdf == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kdf.HashFunctions, ValidHashFunctions, "IKEv1 HashFunctions"));
        }

        private void ValidateKdfMethod(TlsV10_11Kdf kdf, int i, List<string> errorResults)
        {
            // There's nothing to validate for this one
        }

        private void ValidateKdfMethod(TlsV12Kdf kdf, int i, List<string> errorResults)
        {
            if (kdf == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kdf.HashFunctions, ValidHashFunctionsTlsV12, "TLS v1.2 HashFunctions"));
        }
        #endregion IkeV2

        private void ValidateFixedInfoPattern(string fixedInfoPattern, List<string> errorResults, List<string> requiredPieces)
        {
            if (string.IsNullOrEmpty(fixedInfoPattern))
            {
                errorResults.Add($"{nameof(fixedInfoPattern)} was not provided.");
                return;
            }

            const string fiRegex = @"^((?!(l|iv|salt|uPartyInfo|vPartyInfo|context|algorithmId|label|literal\[[0-9a-fA-F]+\])).)+$";

            if (requiredPieces != null)
            {
                foreach (var requiredPiece in requiredPieces)
                {
                    if (!fixedInfoPattern.Contains(requiredPiece))
                    {
                        errorResults.Add($"{nameof(fixedInfoPattern)} missing required piece of {requiredPiece}");
                    }
                }
            }

            var fiPieces = fixedInfoPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (fiPieces?.Length == 0)
            {
                errorResults.Add($"Invalid {nameof(fixedInfoPattern)} {fixedInfoPattern}");
            }
            foreach (var fiPiece in fiPieces)
            {
                Regex regex = new Regex(fiRegex, RegexOptions.IgnoreCase);
                if (regex.IsMatch(fiPiece))
                {
                    errorResults.Add($"{nameof(fixedInfoPattern)} has invalid element {fiPiece}");
                }
            }
        }
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
            ValidateEncoding(schemeKtsCapabilities.Encoding, errorResults);
        }

        private void ValidateAssociatedDataPattern(string associatedDataPattern, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(associatedDataPattern))
            {
                return;
            }

            const string fiRegex = @"^((?!(l|uPartyInfo|vPartyInfo|literal\[[0-9a-fA-F]+\])).)+$";

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
            var schemeRequiresMac = KeyGenerationRequirementsHelper.IfcKcSchemes.Contains(scheme);

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
            ValidateMacKeyLen(l, macOptions.Kmac128, errorResults);
            ValidateMacKeyLen(l, macOptions.Kmac256, errorResults);
        }

        private void ValidateMacKeyLen(int schemeL, MacOptionsBase macMethod, List<string> errorResults)
        {
            if (macMethod == null)
            {
                return;
            }

            var keyConfirmationMacDetails =
                KeyGenerationRequirementsHelper.GetKeyConfirmationMacDetails(macMethod.MacType);

            if (schemeL <= macMethod.KeyLen)
            {
                errorResults.Add($"Provided {nameof(schemeL)} value does not contain enough keying material to perform key confirmation with {macMethod.MacType}");
            }

            if ((macMethod.KeyLen - schemeL) % BitString.BITSINBYTE != 0)
            {
                errorResults.Add($"Provided {nameof(macMethod.KeyLen)} - {nameof(schemeL)} mod 8 should equal 0.");
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

        private void ValidateEncoding(FixedInfoEncoding[] encoding, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(encoding, ValidEncodingTypes, "Encoding type"));
        }

        private void ValidateKeys(Parameters parameters, List<string> errorResults)
        {
            if (parameters.IsSample)
            {
                // Cannot use the public keys provided for a sample, as we need the private keys as well
                parameters.IutKeys = null;
                return;
            }

            if (!parameters.IsSample && (parameters.IutKeys == null || !parameters.IutKeys.Any()))
            {
                errorResults.Add($"The IUT shall provide {nameof(parameters.IutKeys)} for each fixed exponent/modulo size registered, as well as public keys for use for each modulo size for random exponents (when applicable).");
                return;
            }

            if (parameters.IutKeys.Any(a => a.PrivateKeyFormat == IfcKeyGenerationMethod.None))
            {
                errorResults.Add($"{nameof(IutKeys.PrivateKeyFormat)} not provided for one or more {nameof(IutKeys)}");
            }

            // Check for valid E values
            foreach (var key in parameters.IutKeys)
            {
                if (!RsaKeyHelper.IsValidExponent(key.E))
                {
                    errorResults.Add($"invalid {nameof(key.E)} value of {key.E}");
                }
            }

            // collection of the product of fixed public exponent, key generation method, and modulo.
            var exponentKeyGenMethodModulo = parameters
                .Scheme.GetRegisteredSchemes()
                .SelectMany(s => s.KeyGenerationMethods.GetRegisteredKeyGenerationMethods()
                    .Select(s2 => new
                    {
                        s2.FixedPublicExponent,
                        s2.KeyGenerationMethod,
                        s2.Modulo
                    }));

            // Make sure there are IUT provided keys meeting the product.
            foreach (var item in exponentKeyGenMethodModulo)
            {
                if (item.FixedPublicExponent == 0)
                {
                    foreach (var modulo in item.Modulo)
                    {
                        if (!parameters.IutKeys.TryFirst(w =>
                            w.PrivateKeyFormat == item.KeyGenerationMethod &&
                            w.N.ExactBitLength() == modulo,
                            out var result))
                        {
                            errorResults.Add($"Unable to find candidate key from {nameof(parameters.IutKeys)} matching {nameof(IutKeys.PrivateKeyFormat)} ({item.KeyGenerationMethod}) and {nameof(modulo)} ({modulo})");
                        }
                    }
                }
                else
                {
                    foreach (var modulo in item.Modulo)
                    {
                        if (!parameters.IutKeys.TryFirst(w =>
                                w.PrivateKeyFormat == item.KeyGenerationMethod &&
                                w.N.ExactBitLength() == modulo &&
                                w.E == item.FixedPublicExponent,
                            out var result))
                        {
                            errorResults.Add($"Unable to find candidate key from {nameof(parameters.IutKeys)} matching {nameof(IutKeys.PrivateKeyFormat)} ({item.KeyGenerationMethod}), {nameof(modulo)} ({modulo}), and {nameof(IutKeys.E)} ({new BitString(item.FixedPublicExponent.ToHex())})");
                        }
                    }
                }
            }
        }
    }
}