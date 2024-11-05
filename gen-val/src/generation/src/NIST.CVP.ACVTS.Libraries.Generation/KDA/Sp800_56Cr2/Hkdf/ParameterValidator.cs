using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        #region ValidationStatics

        public static readonly (HashFunctions hashFunction, int minimumL)[] MapHashFunctionsMinimumL =
        {
            (HashFunctions.Sha1, 112),
            (HashFunctions.Sha2_d224, 112),
            (HashFunctions.Sha2_d256, 112),
            (HashFunctions.Sha2_d384, 112),
            (HashFunctions.Sha2_d512, 112),
            (HashFunctions.Sha2_d512t224, 112),
            (HashFunctions.Sha2_d512t256, 112),
            (HashFunctions.Sha3_d224, 112),
            (HashFunctions.Sha3_d256, 112),
            (HashFunctions.Sha3_d384, 112),
            (HashFunctions.Sha3_d512, 112),
        };

        public static int GetHashInputBlockSize(HashFunctions hmacAlg)
        {
            switch (hmacAlg)
            {
                case HashFunctions.Sha1:
                    return 512;
                case HashFunctions.Sha2_d224:
                    return 512;
                case HashFunctions.Sha2_d512t224:
                    return 1024;
                case HashFunctions.Sha3_d224:
                    return 1152;
                case HashFunctions.Sha2_d256:
                    return 512;
                case HashFunctions.Sha2_d512t256:
                    return 1024;
                case HashFunctions.Sha3_d256:
                    return 1088;
                case HashFunctions.Sha2_d384:
                    return 1024;
                case HashFunctions.Sha3_d384:
                    return 832;
                case HashFunctions.Sha2_d512:
                    return 1024;
                case HashFunctions.Sha3_d512:
                    return 576;
            }

            return 0;
        }
        
        private static readonly int MaximumL = 2048;

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

        private static readonly string[] RequiredFixedInfoPatternPieces =
        {
            "uPartyInfo",
            "vPartyInfo"
        };

        private static readonly FixedInfoEncoding[] ValidEncodingTypes =
        {
            FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
        };

        private AlgoMode _algoMode;

        #endregion ValidationStatics

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            ValidateAlgoMode(parameters, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateHashAlgs(parameters.HmacAlg, errors);

            // we can't validate L against invalid hash algs
            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateMacSaltMethods(parameters, errors);
            ValidateFixedInfoPattern(parameters.FixedInfoPattern, errors);
            ValidateFixedInfoEncoding(parameters.Encoding, errors);
            ValidateZ(parameters.Z, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateL(parameters, errors);
            ValidateUsesHybridSharedSecret(parameters, errors);
            // saltLens is something we added after the fact... was not baked in from the get-go
            ValidateSaltLens(parameters, errors);
            
            return new ParameterValidateResponse(errors);
        }

        private void ValidateMacSaltMethods(Parameters parameters, List<string> errors)
        {
            if (parameters.MacSaltMethods == null)
            {
                errors.Add($"{nameof(parameters.MacSaltMethods)} is missing and is a required setting.");
            }
        }

        private void ValidateAlgoMode(Parameters parameters, List<string> errors)
        {
            try
            {
                _algoMode =
                    AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

                if (_algoMode != AlgoMode.KDA_HKDF_Sp800_56Cr2)
                {
                    errors.Add("Invalid algo/mode/revision for generator.");
                }
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }
        }

        private void ValidateHashAlgs(HashFunctions[] hmacAlg, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(hmacAlg,
                EnumHelpers.GetEnumsWithoutDefault<HashFunctions>()
                    .ToArray(), "hmacAlg"));
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

                if (!ValidFixedInfoPatternPieces.Contains(fiPiece))
                {
                    errorResults.Add($"Invalid portion of fixedInfoPattern: {fiPiece}");
                }
            }
        }

        private void ValidateFixedInfoEncoding(FixedInfoEncoding[] fixedInfoEncoding, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(fixedInfoEncoding, ValidEncodingTypes, "Encoding type"));
        }

        private void ValidateL(Parameters param, List<string> errors)
        {
            // Ensure the L value meets the minimum L per auxFunction
            foreach (var hmacAlg in param.HmacAlg)
            {
                var auxFunctionMinimumL = MapHashFunctionsMinimumL
                    .First(w => w.hashFunction == hmacAlg).minimumL;

                if (param.L < auxFunctionMinimumL)
                {
                    errors.Add($"Provided 'l' value of {param.L} does not meet the minimum l value of {auxFunctionMinimumL} for the function {hmacAlg}.");
                }
            }

            if (param.L > MaximumL)
            {
                errors.Add($"Provided 'l' value of {param.L} exceeds that maximum testable l value of {MaximumL}.");
            }
        }

        private void ValidateZ(MathDomain z, List<string> errors)
        {
            if (!ValidateDomain(z, errors, "z", 224, 65536))
            {
                return;
            }
            var modCheck = ValidateMultipleOf(z, 8, "z");
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
            // Used here due to 'usesShared/auxShared' used with registrations
            var usesName = "usesHybridSharedSecret";
            var auxName = "auxSharedSecretLen";
            
            try
            {
                // the usesHybridSharedSecret registration property is required for 56Cr2 testing
                if (parameters.UsesHybridSharedSecret == null)
                {
                    errors.Add($"The {usesName} registration property is required for algo/mode/revision {_algoMode} testing, but was not provided.");
                }
                else if (parameters.UsesHybridSharedSecret == true)
                {
                    // if UsesHybridSharedSecret, then AuxSharedSecretLen can't be null
                    if (parameters.AuxSharedSecretLen == null)
                    {
                        errors.Add(
                            $"For algo/mode/revision {_algoMode}, when {usesName}:true," +
                            $" the {auxName} registration property must be provided.");                             
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

        private void ValidateSaltLens(Parameters parameters, List<string> errors)
        {
            // saltLens is an optional registration property
            if (parameters.SaltLens != null)
            {
                var saltLenMinMax = parameters.SaltLens.GetDomainMinMax();
                
                // Per SP 800-56Cr2 Section 4.2, the length of the chosen salt must be at least 8 bits
                if (saltLenMinMax.Minimum < 8)
                    errors.Add($"Values of {nameof(parameters.SaltLens)} must be >= 8 bits, but the value {saltLenMinMax.Minimum} was supplied for {nameof(parameters.SaltLens)}.");
                
                // Per SP 800-56Cr2 Section 4.2, the length of the chosen salt must be "no greater than the length of a
                // single input block to the hash function, hash, used to implement HMAC-hash."
                int maxHashInputBlockSize = 0;
                // 1) For each hmacAlg, there must be a saltLen that is no greater than the length of the hash's input block size.
                foreach (var hmacAlg in parameters.HmacAlg)
                {
                    var hashInputBlockSize = GetHashInputBlockSize(hmacAlg);
                    // figure out the largest hash input block size from hmacAlgs for below/later
                    if (hashInputBlockSize > maxHashInputBlockSize) maxHashInputBlockSize = hashInputBlockSize;
                    if (saltLenMinMax.Minimum > hashInputBlockSize)
                        errors.Add($"{nameof(parameters.SaltLens)} must be <= the input block size of {hmacAlg}, but {nameof(parameters.SaltLens)} contains no value <= {hashInputBlockSize}.");    
                }
                // 2) For each saltLen there must be an hmacAlg whose input block size is >= equal to that saltLen
                if (saltLenMinMax.Maximum > maxHashInputBlockSize)
                    errors.Add($"{nameof(parameters.SaltLens)} must be <= the input block size of the hash function used to implement {nameof(parameters.HmacAlg)}. The {nameof(parameters.SaltLens)} value of {saltLenMinMax.Maximum} is larger than the largest input block size of {nameof(parameters.HmacAlg)}, i.e., {maxHashInputBlockSize}.");                    
            }
        }
    }
}
