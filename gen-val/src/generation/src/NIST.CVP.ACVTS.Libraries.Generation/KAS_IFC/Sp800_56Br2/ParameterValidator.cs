using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
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

        public static readonly KasHashAlg[] ValidHashAlgs =
        {
            KasHashAlg.SHA1,
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

        public static readonly (KdaOneStepAuxFunction functionName, bool requiresSaltLength)[] ValidAuxMethods =
        {
            (KdaOneStepAuxFunction.SHA1, false),
            (KdaOneStepAuxFunction.SHA2_D224, false),
            (KdaOneStepAuxFunction.SHA2_D256, false),
            (KdaOneStepAuxFunction.SHA2_D384, false),
            (KdaOneStepAuxFunction.SHA2_D512, false),
            (KdaOneStepAuxFunction.SHA2_D512_T224, false),
            (KdaOneStepAuxFunction.SHA2_D512_T256, false),
            (KdaOneStepAuxFunction.SHA3_D224, false),
            (KdaOneStepAuxFunction.SHA3_D256, false),
            (KdaOneStepAuxFunction.SHA3_D384, false),
            (KdaOneStepAuxFunction.SHA3_D512, false),
            (KdaOneStepAuxFunction.HMAC_SHA1, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D224, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D256, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D384, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D512, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D512_T224, true),
            (KdaOneStepAuxFunction.HMAC_SHA2_D512_T256, true),
            (KdaOneStepAuxFunction.HMAC_SHA3_D224, true),
            (KdaOneStepAuxFunction.HMAC_SHA3_D256, true),
            (KdaOneStepAuxFunction.HMAC_SHA3_D384, true),
            (KdaOneStepAuxFunction.HMAC_SHA3_D512, true),
            (KdaOneStepAuxFunction.KMAC_128, true),
            (KdaOneStepAuxFunction.KMAC_256, true),
        };

        private static readonly AlgoMode[] ValidAlgoModes = new[]
        {
            AlgoMode.KAS_IFC_Sp800_56Br2,
            AlgoMode.KTS_IFC_Sp800_56Br2
        };

        private static readonly IfcKeyGenerationMethod[] ValidKeyGenerationMethods =
            EnumHelpers.GetEnumsWithoutDefault<IfcKeyGenerationMethod>().ToArray();

        private static readonly MacSaltMethod[] ValidSaltGenerationMethods = { MacSaltMethod.Default, MacSaltMethod.Random };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes =
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };

        private static readonly int[] ValidModulo = ParameterSetDetails.RsaModuloDetails.Keys.ToArray();

        private static readonly int[] ValidAesKeyLengths = new[] { 128, 192, 256 };
        private static readonly int MinimumL = 112;
        private static readonly int MaximumL = 1024;

        private static readonly string[] ValidFixedInfoPatternPieces =
        {
            "l",
            "iv",
            "salt",
            "uPartyInfo",
            "vPartyInfo",
            "context",
            "algorithmId",
            "label"
        };

        private static readonly string[] ValidAssociatedDataPatternPieces =
        {
            "l",
            "uPartyInfo",
            "vPartyInfo",
            "context",
            "algorithmId",
            "label"
        };
        #endregion Validation statics

        private AlgoMode _algoMode;
        private bool _isKasScheme;
        private bool _isKtsScheme;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();
            _algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            if (_algoMode == AlgoMode.KAS_IFC_Sp800_56Br2)
            {
                _isKasScheme = true;
            }

            if (_algoMode == AlgoMode.KTS_IFC_Sp800_56Br2)
            {
                _isKtsScheme = true;
            }

            if (!ValidAlgoModes.Contains(_algoMode))
            {
                errorResults.Add("Invalid Algorithm, mode, revision combination.");
                return new ParameterValidateResponse(errorResults);
            }

            ValidateFunction(parameters, errorResults);
            ValidateIutId(parameters, errorResults);
            ValidateSchemes(parameters, errorResults);
            ValidateKeyGenerationMethod(parameters, errorResults);
            ValidateModulo(parameters.Modulo, errorResults);

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

        private void ValidateIutId(Parameters parameters, List<string> errorResults)
        {
            if (parameters.IutId == null || parameters.IutId.BitLength == 0)
            {
                errorResults.Add($"{nameof(parameters.IutId)} was not supplied.");
            }
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
                case AlgoMode.KAS_IFC_Sp800_56Br2:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(KeyGenerationRequirementsHelper.IfcKtsSchemes).Any())
                    {
                        errorResults.Add(invalidSchemeMessage);
                    }
                    break;
                case AlgoMode.KTS_IFC_Sp800_56Br2:
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
            ValidateKdfMethods(scheme.KdfMethods, scheme.L, errorResults);
            ValidateKtsMethods(scheme.KtsMethod, errorResults);
            ValidateMacMethods(scheme.Scheme, scheme.MacMethods, scheme.L, errorResults);
        }

        private void ValidateKeyAgreementRoles(KeyAgreementRole[] schemeRoles, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }

        private void ValidateKeyGenerationMethod(Parameters parameters, List<string> errorResults)
        {
            if (errorResults.AddIfNotNullOrEmpty(
                ValidateArray(parameters.KeyGenerationMethods, ValidKeyGenerationMethods,
                "keyGenerationMethods")))
            {
                return;
            }

            // Validate a fixed public exponent is provided for "1" key generation methods
            var methodsRequiringFixedPublicExponent = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_basic,
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor
            };

            var requiresFixedPublicExponent = parameters.KeyGenerationMethods.Intersect(methodsRequiringFixedPublicExponent).Any();
            if (requiresFixedPublicExponent)
            {
                if (!RsaKeyHelper.IsValidExponent(parameters.PublicExponent))
                {
                    errorResults.Add("Valid fixed public exponent required for the registered key generation modes.");
                }
            }

            // If only "2" key generation methods are supplied, ensure a public exponent is not supplied.
            var methodsNotRequiringFixedPublicExponent = new[]
            {
                IfcKeyGenerationMethod.RsaKpg2_basic,
                IfcKeyGenerationMethod.RsaKpg2_crt,
                IfcKeyGenerationMethod.RsaKpg2_primeFactor
            };

            if (!requiresFixedPublicExponent &&
                parameters.KeyGenerationMethods.Intersect(methodsNotRequiringFixedPublicExponent).Any() &&
                parameters.PublicExponent != 0)
            {
                errorResults.Add("Unexpected fixed public exponent");
            }
        }

        private void ValidateModulo(int[] modulo, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(modulo, ValidModulo, "Modulus"));
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
                errorResults.Add("KDF object was not found.");
                return;
            }

            var registeredKdfs = schemeKdfMethods.GetRegisteredKdfMethods();

            if (!registeredKdfs.Any())
            {
                errorResults.Add("At least one KDF is required.");
                return;
            }

            ValidateKdfMethod(schemeKdfMethods.OneStepKdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.OneStepNoCounterKdf, l, errorResults);
            ValidateKdfMethod(schemeKdfMethods.TwoStepKdf, l, errorResults);
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
                    errorResults.AddIfNotNullOrEmpty(
                        ValidateArray(
                            auxFunction.MacSaltMethods,
                            new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                            "Salt Method OneStep KDF"));
                }
            }
        }
        #endregion OneStepKdf

        #region OneStepNoCounterKdf
        private void ValidateKdfMethod(OneStepNoCounterKdf kdf, int l, List<string> errorResults)
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

        private void ValidateAuxFunction(AuxFunctionNoCounter[] auxFunctions, List<string> errorResults)
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
                    errorResults.AddIfNotNullOrEmpty(
                        ValidateArray(
                            auxFunction.MacSaltMethods,
                            new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                            "Salt Method OneStep KDF"));
                }

                var outputLen = EnumMapping.GetMaxOutputLengthOfDkmForOneStepAuxFunction(auxFunction.AuxFunctionName);

                if (auxFunction.L % BitString.BITSINBYTE != 0)
                {
                    errorResults.Add($"{nameof(auxFunction.L)} mod 8 should equal 0.");
                }

                if (auxFunction.L < 112)
                {
                    errorResults.Add($"Provided {nameof(auxFunction.L)} of {auxFunction.L} must be at least 112 bits.");
                }

                if (auxFunction.L > outputLen)
                {
                    errorResults.Add($"For a oneStepNoCounterKdf the provided {nameof(auxFunction.L)} value of {auxFunction.L} may not exceed the output length of the function {outputLen}.");
                }
            }
        }
        #endregion OneStepNoCounterKdf

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

                errorResults.AddIfNotNullOrEmpty(
                    ValidateArray(
                        capability.MacSaltMethods,
                        new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                        "Salt Method OneStep KDF"));
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

            Regex notHexRegex = new Regex(@"[^0-9a-fA-F]", RegexOptions.IgnoreCase);
            string literalStart = "literal[";
            string literalEnd = "]";

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

            var allUniquePieces = fiPieces
                .GroupBy(gb => gb)
                .All(a => a.Count() == 1);

            if (!allUniquePieces)
            {
                errorResults.Add($"Duplicate pieces of {nameof(fixedInfoPattern)} found; pieces should be unique.");
            }

            foreach (var fiPiece in fiPieces)
            {
                if (fiPiece.StartsWith(literalStart) && fiPiece.EndsWith(literalEnd))
                {
                    var tempLiteral = fiPiece.Replace(literalStart, string.Empty);
                    tempLiteral = tempLiteral.Replace(literalEnd, string.Empty);

                    if (notHexRegex.IsMatch(tempLiteral))
                    {
                        errorResults.Add("literal element of fixedInfoPattern contained non hex values.");
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
                    errorResults.Add($"Invalid portion of fixedInfoPattern: {fiPiece}");
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

            if (schemeKtsCapabilities == null)
            {
                errorResults.Add("KTS method is required but was not provided.");
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeKtsCapabilities.HashAlgs, ValidHashAlgs, "KTS HashAlgs"));
            ValidateAssociatedDataPattern(schemeKtsCapabilities, errorResults);
            ValidateEncoding(schemeKtsCapabilities.Encoding, errorResults);
        }

        private void ValidateAssociatedDataPattern(KtsMethod ktsMethod, List<string> errorResults)
        {
            if (!ktsMethod.SupportsNullAssociatedData && string.IsNullOrEmpty(ktsMethod.AssociatedDataPattern))
            {
                errorResults.Add($"Implementation must supply an {nameof(ktsMethod.AssociatedDataPattern)} if {nameof(ktsMethod.SupportsNullAssociatedData)} is false.");
                return;
            }

            if (string.IsNullOrEmpty(ktsMethod.AssociatedDataPattern))
            {
                return;
            }

            Regex notHexRegex = new Regex(@"[^0-9a-fA-F]", RegexOptions.IgnoreCase);
            string literalStart = "literal[";
            string literalEnd = "]";

            var fiPieces = ktsMethod.AssociatedDataPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (fiPieces?.Length == 0)
            {
                errorResults.Add($"Invalid {nameof(ktsMethod.AssociatedDataPattern)} {ktsMethod.AssociatedDataPattern}");
            }

            var allUniquePieces = fiPieces
                .GroupBy(gb => gb)
                .All(a => a.Count() == 1);

            if (!allUniquePieces)
            {
                errorResults.Add($"Duplicate pieces of {nameof(ktsMethod.AssociatedDataPattern)} found; pieces should be unique.");
            }

            foreach (var fiPiece in fiPieces)
            {
                if (fiPiece.StartsWith(literalStart) && fiPiece.EndsWith(literalEnd))
                {
                    var tempLiteral = fiPiece.Replace(literalStart, string.Empty);
                    tempLiteral = tempLiteral.Replace(literalEnd, string.Empty);

                    if (notHexRegex.IsMatch(tempLiteral))
                    {
                        errorResults.Add("literal element of fixedInfoPattern contained non hex values.");
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

                if (!ValidAssociatedDataPatternPieces.Contains(fiPiece))
                {
                    errorResults.Add($"Invalid portion of fixedInfoPattern: {fiPiece}");
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
            ValidateMacKeyLen(l, macOptions.HmacSha1, errorResults);
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
    }
}
