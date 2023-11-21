using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static readonly string[] ValidFixedInfoPatternPieces_Cr2 =
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

        private static readonly string[] RequiredFixedInfoPatternPieces =
        {
            "uPartyInfo",
            "vPartyInfo"
        };

        private static readonly MacSaltMethod[] ValidSaltGenerationMethods = { MacSaltMethod.Default, MacSaltMethod.Random };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes =
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };

        private AlgoMode _algoMode;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            ValidateAlgoMode(parameters, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateZ(parameters.Z, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateKdfMethod(parameters, errors);
            ValidateUsesHybridSharedSecret(parameters, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateAlgoMode(Parameters parameters, List<string> errors)
        {
            try
            {
                _algoMode =
                    AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

                if (_algoMode != AlgoMode.KDA_TwoStep_Sp800_56Cr2)
                {
                    errors.Add("Invalid algo/mode/revision for generator.");
                }
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }
        }

        private void ValidateKdfMethod(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Capabilities == null || parameters.Capabilities.Length == 0)
            {
                errorResults.Add($"{nameof(parameters.Capabilities)} may not be null or empty");
                return;
            }

            var kdfTwoStepValidator = new KDF.v1_0.ParameterValidator();
            var kdfParam = new KDF.v1_0.Parameters()
            {
                Algorithm = "KDF",
                Revision = "1.0",
                Capabilities = parameters.Capabilities
            };

            var validate = kdfTwoStepValidator.Validate(kdfParam);
            if (!validate.Success)
            {
                errorResults.AddRangeIfNotNullOrEmpty(validate.Errors);
                return;
            }

            foreach (var capability in parameters.Capabilities)
            {
                ValidateFixedInfoPattern(capability.FixedInfoPattern, errorResults);
                ValidateEncoding(capability.Encoding, errorResults);
                errorResults.AddIfNotNullOrEmpty(ValidateArray(capability.MacSaltMethods, ValidSaltGenerationMethods, nameof(MacSaltMethod)));

                // Ensure the L value is contained within the capabilities domain
                if (!capability.SupportedLengths.IsWithinDomain(parameters.L))
                {
                    errorResults.Add($"Provided {nameof(parameters.L)} value of {parameters.L} was not contained within the {nameof(capability.SupportedLengths)} domain.");
                }

                errorResults.AddIfNotNullOrEmpty(
                    ValidateArray(
                        capability.MacSaltMethods,
                        new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                        "Salt Method OneStep KDF"));
            }
        }

        private void ValidateFixedInfoPattern(string fixedInfoPattern, List<string> errorResults)
        {
            if (string.IsNullOrEmpty(fixedInfoPattern))
            {
                errorResults.Add($"{nameof(fixedInfoPattern)} was not provided.");
                return;
            }

            Regex notHexRegex = new Regex(@"[^0-9a-fA-F]", RegexOptions.IgnoreCase);
            string literalStart = "literal[";
            string literalEnd = "]";

            var fiPieces = fixedInfoPattern.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (fiPieces?.Length == 0)
            {
                errorResults.Add($"Invalid {nameof(fixedInfoPattern)} {fixedInfoPattern}");
            }

            var needsRequiredPieces = true;

            // entropyBits fixed info pattern can bypass the normal "requiredPieces"
            const string entropyBits = "entropyBits";
            if (fixedInfoPattern.Contains(entropyBits))
            {
                const int maxEntropyBits = 4096;
                //needsRequiredPieces = false;

                var entropyBitsPortion = fiPieces.First(w => w.Contains(entropyBits));
                entropyBitsPortion = entropyBitsPortion
                    .Replace("||", "")
                    .Replace($"{entropyBits}[", "")
                    .Replace("]", "");

                var entropyBitsParse = int.TryParse(entropyBitsPortion, out var entropyBitsLen);

                if (entropyBitsParse)
                {
                    if (entropyBitsLen > maxEntropyBits)
                    {
                        errorResults.Add($"{nameof(entropyBits)} exceeded maximum allowed value of {maxEntropyBits}");
                    }

                    if (entropyBitsLen % BitString.BITSINBYTE != 0)
                    {
                        errorResults.Add($"{nameof(entropyBits)} must be on the byte boundary.");
                    }
                }
                else
                {
                    errorResults.Add($"{nameof(entropyBits)} could not be parsed.");
                }
            }

            if (needsRequiredPieces)
            {
                foreach (var requiredPiece in RequiredFixedInfoPatternPieces)
                {
                    if (!fixedInfoPattern.Contains(requiredPiece))
                    {
                        errorResults.Add($"{nameof(fixedInfoPattern)} missing required piece of {requiredPiece}");
                    }
                }
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

                if (fiPiece.Contains("literal") || fiPiece.Contains("entropyBits")) continue;

                if (!ValidFixedInfoPatternPieces_Cr2.Contains(fiPiece))
                {
                    errorResults.Add($"Invalid portion of fixedInfoPattern: {fiPiece}");
                }
            }
        }

        private void ValidateEncoding(FixedInfoEncoding[] encoding, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(encoding, ValidEncodingTypes, "Encoding type"));
        }

        private void ValidateZ(MathDomain z, List<string> errors)
        {
            if (!ValidateDomain(z, errors, "z", 224, 65536))
            {
                return;
            }
            var modCheck = ValidateMultipleOf(z, 8, "z mod");
            if (!string.IsNullOrEmpty(modCheck))
            {
                errors.Add(modCheck);
            }
        }

        private void ValidateAuxSSLen(MathDomain AuxSharedSecretLen, List<string> errors)
        {
            if (!ValidateDomain(AuxSharedSecretLen, errors, "auxSharedSecretLen", 112, 65536))
            {
                return;
            }
            var modCheck = ValidateMultipleOf(AuxSharedSecretLen, 8, "auxSharedSecretLen");
            if (!string.IsNullOrEmpty(modCheck))
            {
                errors.Add(modCheck);
            }
        }

        private void ValidateUsesHybridSharedSecret(Parameters parameters, List<string> errors)
        {
            try
            {
                // the usesHybridSharedSecret registration property is required for 56Cr2 testing
                if (parameters.UsesHybridSharedSecret == null)
                {
                    errors.Add($"The {nameof(parameters.UsesHybridSharedSecret)} registration property is required for algo/mode/revision {_algoMode} testing, but was not provided.");
                }
                else if (parameters.UsesHybridSharedSecret == true)
                {
                    // if UsesHybridSharedSecret, then AuxSharedSecretLen can't be null
                    if (parameters.AuxSharedSecretLen == null)
                    {
                        errors.Add(
                            $"For algo/mode/revision {_algoMode}, when {nameof(parameters.UsesHybridSharedSecret)}:true," +
                            $" the {nameof(parameters.AuxSharedSecretLen)} registration property must be provided.");                             
                    }
                    // validate auxSharedSecretLen
                    else
                    {
                        ValidateAuxSSLen(parameters.AuxSharedSecretLen, errors);
                    }
                }                    
                // If the usesHybridSharedSecret registration property equals false, but the auxSharedSecretLen
                // registration parameter is present 
                else if (parameters.UsesHybridSharedSecret == false && parameters.AuxSharedSecretLen != null)
                {
                    errors.Add($"The {nameof(parameters.AuxSharedSecretLen)} registration property may not be used " +
                               $"except in combination with {nameof(parameters.UsesHybridSharedSecret)}:true for " +
                               $"algo/mode/revision {_algoMode}");                    
                }
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }
        }
    }
}
