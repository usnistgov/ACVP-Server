using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Exceptions;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {

        #region Validation statics

        private static readonly AlgoMode[] ValidAlgoModes =
        {
            AlgoMode.KAS_ECC_Sp800_56Ar3,
            AlgoMode.KAS_FFC_Sp800_56Ar3,
        };

        private static readonly KasDpGeneration[] ValidFfcDpGeneration =
        {
            KasDpGeneration.Modp2048,
            KasDpGeneration.Modp3072,
            KasDpGeneration.Modp4096,
            KasDpGeneration.Modp6144,
            KasDpGeneration.Modp8192,
            KasDpGeneration.Ffdhe2048,
            KasDpGeneration.Ffdhe3072,
            KasDpGeneration.Ffdhe4096,
            KasDpGeneration.Ffdhe6144,
            KasDpGeneration.Ffdhe8192,
            KasDpGeneration.Fb,
            KasDpGeneration.Fc,
        };
        
        private static readonly KasDpGeneration[] ValidEccDpGeneration = 
        {
            KasDpGeneration.P192,
            KasDpGeneration.P224,
            KasDpGeneration.P256,
            KasDpGeneration.P384,
            KasDpGeneration.P521,
            KasDpGeneration.K163,
            KasDpGeneration.K233,
            KasDpGeneration.K283,
            KasDpGeneration.K409,
            KasDpGeneration.K571,
            KasDpGeneration.B163,
            KasDpGeneration.B233,
            KasDpGeneration.B283,
            KasDpGeneration.B409,
            KasDpGeneration.B571,
        };

        private static readonly KasScheme[] ValidEccSchemes = 
        {
            KasScheme.EccEphemeralUnified,
            KasScheme.EccFullMqv,
            KasScheme.EccFullUnified,
            KasScheme.EccOnePassDh,
            KasScheme.EccOnePassMqv,
            KasScheme.EccOnePassUnified,
            KasScheme.EccStaticUnified,
        };
        
        private static readonly KasScheme[] ValidFfcSchemes = 
        {
            KasScheme.FfcDhEphem,
            KasScheme.FfcDhHybrid1,
            KasScheme.FfcDhHybridOneFlow,
            KasScheme.FfcDhOneFlow,
            KasScheme.FfcDhStatic,
            KasScheme.FfcMqv1,
            KasScheme.FfcMqv2,
        };
        
        public static readonly KasScheme[] InvalidKcSchemes =
        {
            KasScheme.EccEphemeralUnified,
            KasScheme.FfcDhEphem
        };

        public static readonly KeyConfirmationRole[] ValidKeyConfirmationRoles =
        {
            KeyConfirmationRole.Provider,
            KeyConfirmationRole.Recipient,
        };

        public static readonly KeyConfirmationDirection[] ValidKeyConfirmationDirections =
        {
            KeyConfirmationDirection.Unilateral,
            KeyConfirmationDirection.Bilateral
        };
        
        public static readonly string[] ValidFunctions =
        {
            "keyPairGen",
            "fullVal",
            "partialVal",
            "keyRegen",
        };

        public static readonly KeyAgreementRole[] ValidKeyAgreementRoles =
        {
            KeyAgreementRole.InitiatorPartyU,
            KeyAgreementRole.ResponderPartyV
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

        private static readonly MacSaltMethod[] ValidSaltGenerationMethods = { MacSaltMethod.Default, MacSaltMethod.Random };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes = 
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };

        private static readonly int[] ValidAesKeyLengths = { 128, 192, 256 };
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
        #endregion Validation statics

        private AlgoMode _algoMode;

        private bool _isFfcScheme;
        private bool _isEccScheme;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();
            
            ValidateAlgoMode(parameters, errorResults);

            if (errorResults.Count != 0)
            {
                return new ParameterValidateResponse(errorResults);
            }
            
            ValidateFunction(parameters, errorResults);
            ValidateIutId(parameters, errorResults);
            ValidateDomainParameterGeneration(parameters, errorResults);
            ValidateSchemes(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateAlgoMode(Parameters parameters, List<string> errorResults)
        {
            _algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            if (_algoMode == AlgoMode.KAS_ECC_Sp800_56Ar3)
            {
                _isEccScheme = true;
            }

            if (_algoMode == AlgoMode.KAS_FFC_Sp800_56Ar3)
            {
                _isFfcScheme = true;
            }

            if (!ValidAlgoModes.Contains(_algoMode))
            {
                errorResults.Add("Invalid Algorithm, mode, revision combination.");
            }
        }

        private void ValidateFunction(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Function == null || parameters.Function.Length == 0)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Function, ValidFunctions, "Functions"));
        }
        
        private void ValidateDomainParameterGeneration(Parameters parameters, List<string> errorResults)
        {
            if (_isEccScheme)
            {
                errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.DomainParameterGenerationMethods,
                    ValidEccDpGeneration, nameof(parameters.DomainParameterGenerationMethods)));
            }
            
            if (_isFfcScheme)
            {
                errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.DomainParameterGenerationMethods,
                    ValidFfcDpGeneration, nameof(parameters.DomainParameterGenerationMethods)));
            }
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

            var registeredSchemes = parameters.Scheme.GetRegisteredSchemes().ToList();

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

            foreach (var scheme in registeredSchemes)
            {
                ValidateScheme(scheme, errorResults);
            }
        }

        private void ValidateSchemesForAlgoMode(List<string> errorResults, IEnumerable<SchemeBase> registeredSchemes)
        {
            var invalidSchemeMessage = $"Invalid Schemes for registered {_algoMode}";
            
            switch (_algoMode)
            {
                case AlgoMode.KAS_ECC_Sp800_56Ar3:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(ValidFfcSchemes).Any())
                    {
                        errorResults.Add(invalidSchemeMessage);
                    }
                    break;
                case AlgoMode.KAS_FFC_Sp800_56Ar3:
                    if (registeredSchemes.Select(s => s.Scheme).Intersect(ValidEccSchemes).Any())
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
            ValidateKeyConfirmation(scheme, errorResults);
        }

        private void ValidateKeyAgreementRoles(KeyAgreementRole[] schemeRoles, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }

        #region kdfValidation
        private void ValidateKdfMethods(KdfMethods schemeKdfMethods, int l, List<string> errorResults)
        {
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
                    errorResults.AddIfNotNullOrEmpty(
                        ValidateArray(
                            auxFunction.MacSaltMethods,
                            new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                            "Salt Method OneStep KDF"));
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
                        continue;
                    }
                }

                if (!ValidFixedInfoPatternPieces.Contains(fiPiece))
                {
                    errorResults.Add($"Invalid portion of fixedInfoPattern: {fiPiece}");
                }
            }
        }
        #endregion kdfValidation

        private void ValidateKeyConfirmation(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme.KeyConfirmationMethod == null)
            {
                return;
            }
            
            errorResults.AddIfNotNullOrEmpty(ValidateArray(scheme.KeyConfirmationMethod.KeyConfirmationDirections, ValidKeyConfirmationDirections, "KeyConfirmationDirection"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(scheme.KeyConfirmationMethod.KeyConfirmationRoles, ValidKeyConfirmationRoles, "KeyConfirmationRole"));
            ValidateMacMethods(scheme.Scheme, scheme.KeyConfirmationMethod.MacMethods, scheme.L, errorResults);
        }
        
        #region macValidation
        private void ValidateMacMethods(KasScheme scheme, MacMethods macOptions, int l, List<string> errorResults)
        {
            var registeredMacMethods = macOptions.GetRegisteredMacMethods().ToList();
            
            if (!registeredMacMethods.Any())
            {
                errorResults.Add("MacMethods not provided for KeyConfirmation method.");
                return;
            }

            // Certain schemes do not allow for key confirmation
            if (registeredMacMethods.Any() && InvalidKcSchemes.Contains(scheme))
            {
                errorResults.Add($"KeyConfirmation not supported for scheme {scheme}");
                return;
            }
            
            foreach (var macMethod in registeredMacMethods)
            {
                ValidateMacMethod(l, macMethod, errorResults);
            }
        }

        private void ValidateMacMethod(int schemeL, MacOptionsBase macMethod, List<string> errorResults)
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