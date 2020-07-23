using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Exceptions;

namespace NIST.CVP.Generation.KAS_KDF.Hkdf
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		#region ValidationStatics

		public static readonly (HashFunctions hashFunction, int minimumL)[] MapHashFunctionsMinimumL =
		{
			(HashFunctions.Sha2_d224, 448),
			(HashFunctions.Sha2_d256, 512),
			(HashFunctions.Sha2_d384, 768),
			(HashFunctions.Sha2_d512, 1024),
			(HashFunctions.Sha2_d512t224, 448),
			(HashFunctions.Sha2_d512t256, 512),
			(HashFunctions.Sha3_d224, 448),
			(HashFunctions.Sha3_d256, 512),
			(HashFunctions.Sha3_d384, 768),
			(HashFunctions.Sha3_d512, 1024),
		};
		
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
			
			ValidateFixedInfoPattern(parameters.FixedInfoPattern, errors);
			ValidateFixedInfoEncoding(parameters.Encoding, errors);

			ValidateL(parameters, errors);
			ValidateZ(parameters, errors);
			
			return new ParameterValidateResponse(errors);
		}

		private void ValidateAlgoMode(Parameters parameters, List<string> errors)
		{
			ValidateAlgoMode(parameters, new[] { AlgoMode.KAS_KDF_HKDF_Sp800_56Cr1 }, errors);
		}
		
		private void ValidateHashAlgs(HashFunctions[] hmacAlg, List<string> errors)
		{
			ValidateArray(hmacAlg, 
				EnumHelpers.GetEnumsWithoutDefault<HashFunctions>()
					.Except(new []{ HashFunctions.Sha1 })
					.ToArray(), "hmacAlg");
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
            
            foreach (var requiredPiece in RequiredFixedInfoPatternPieces)
            {
	            if (!fiPieces.Contains(requiredPiece))
	            {
		            errorResults.Add($"Required FixedInfo piece {requiredPiece} was not present.");
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
		
		private void ValidateZ(Parameters parameters, List<string> errors)
		{
			if (!ValidateDomain(parameters.Z, errors, "z", 224, 65336))
			{
				return;
			}
			var modCheck = ValidateMultipleOf(parameters.Z, 8, "z mod");
			if (!string.IsNullOrEmpty(modCheck))
			{
				errors.Add(modCheck);
			}
		}
	}
}