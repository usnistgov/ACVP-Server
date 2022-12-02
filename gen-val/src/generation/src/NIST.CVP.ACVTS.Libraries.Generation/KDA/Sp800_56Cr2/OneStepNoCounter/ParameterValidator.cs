using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        #region ValidationStatics

        private AlgoMode _validAlgoMode = AlgoMode.KDA_OneStepNoCounter_Sp800_56Cr2;

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

        private static readonly int MaximumL = 2048;
        private static readonly int MinimumL = 112;

        private static readonly string[] ValidFixedInfoPatternPieces_Cr2 =
        {
            "t",
            "l",
            "iv",
            "salt",
            "uPartyInfo",
            "vPartyInfo",
            "context",
            "algorithmId",
            "label",
            "literal",
            "entropyBits",
        };

        private static readonly string[] RequiredFixedInfoPatternPieces =
        {
            "uPartyInfo",
            "vPartyInfo"
        };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes =
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };
        #endregion ValidationStatics

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!ValidateAlgoMode(parameters, new[] { _validAlgoMode }, errors))
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateAuxFunctions(parameters.AuxFunctions, errors);
            ValidateFixedInfoPattern(parameters.FixedInfoPattern, errors);
            ValidateFixedInfoEncoding(parameters.Encoding, errors);
            ValidateZ(parameters.Z, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            return new ParameterValidateResponse(errors);
        }

        private void ValidateAuxFunctions(AuxFunctionNoCounter[] auxFunctions, List<string> errors)
        {
            if (auxFunctions == null || !auxFunctions.Any())
            {
                errors.Add("at least one AuxFunction is required.");
                return;
            }

            if (errors.AddIfNotNullOrEmpty(ValidateArray(
                auxFunctions.Select(s => s.AuxFunctionName),
                ValidAuxMethods.Select(s => s.functionName),
                $"Aux Function")))
            {
                return;
            }

            foreach (var auxFunction in auxFunctions)
            {
                var needsSalt = ValidAuxMethods.First(w =>
                        w.functionName.Equals(auxFunction.AuxFunctionName))
                    .requiresSaltLength;

                if (needsSalt)
                {
                    errors.AddIfNotNullOrEmpty(
                        ValidateArray(
                            auxFunction.MacSaltMethods,
                            new[] { MacSaltMethod.Default, MacSaltMethod.Random },
                            "Salt Method OneStep KDF"));
                }

                if (auxFunction.L < MinimumL)
                {
                    errors.Add($"Provided {nameof(auxFunction.L)} of {auxFunction.L} is less than the minimum allowed value of {MinimumL} for {auxFunction.AuxFunctionName}.");
                }

                if (auxFunction.L % BitString.BITSINBYTE != 0)
                {
                    errors.Add($"Provided {nameof(auxFunction.L)} of {auxFunction.L} is not mod 8 (byte boundary) for {auxFunction.AuxFunctionName}.");
                }

                var outputLen = EnumMapping.GetMaxOutputLengthOfDkmForOneStepAuxFunction(auxFunction.AuxFunctionName);
                if (auxFunction.L > outputLen)
                {
                    errors.Add($"For a oneStepNoCounterKdf the provided {nameof(auxFunction.L)} value of {auxFunction.L} may not exceed the output length of the function {outputLen} for {auxFunction.AuxFunctionName}.");
                }

                if (auxFunction.L > MaximumL)
                {
                    errors.Add($"Provided {nameof(auxFunction.L)} of {auxFunction.L} is greater than the maximum allowed value of {MaximumL} for {auxFunction}.");
                }
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
                return;
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
                    if (!fiPieces.Contains(requiredPiece))
                    {
                        errorResults.Add($"Required FixedInfo piece {requiredPiece} was not present.");
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

        private void ValidateFixedInfoEncoding(FixedInfoEncoding[] fixedInfoEncoding, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(fixedInfoEncoding, ValidEncodingTypes, "Encoding type"));
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
    }
}
