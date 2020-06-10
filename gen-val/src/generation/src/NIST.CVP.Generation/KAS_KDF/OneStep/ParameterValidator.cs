using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Math.Exceptions;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		#region ValidationStatics
		public static readonly (KasKdfOneStepAuxFunction functionName, bool requiresSaltLength, int minimumL)[] ValidAuxMethods =
		{
			(KasKdfOneStepAuxFunction.SHA2_D224, false, 448),
			(KasKdfOneStepAuxFunction.SHA2_D256, false, 512),
			(KasKdfOneStepAuxFunction.SHA2_D384, false, 768),
			(KasKdfOneStepAuxFunction.SHA2_D512, false, 1024),
			(KasKdfOneStepAuxFunction.SHA2_D512_T224, false, 448),
			(KasKdfOneStepAuxFunction.SHA2_D512_T256, false, 512),
			(KasKdfOneStepAuxFunction.SHA3_D224, false, 448),
			(KasKdfOneStepAuxFunction.SHA3_D256, false, 512),
			(KasKdfOneStepAuxFunction.SHA3_D384, false, 768),
			(KasKdfOneStepAuxFunction.SHA3_D512, false, 1024),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D224, true, 448),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D256, true, 512),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D384, true, 768),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D512, true, 1024),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224, true, 448),
			(KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256, true, 512),
			(KasKdfOneStepAuxFunction.HMAC_SHA3_D224, true, 448),
			(KasKdfOneStepAuxFunction.HMAC_SHA3_D256, true, 512),
			(KasKdfOneStepAuxFunction.HMAC_SHA3_D384, true, 768),
			(KasKdfOneStepAuxFunction.HMAC_SHA3_D512, true, 1024),
			(KasKdfOneStepAuxFunction.KMAC_128, true, 256),
			(KasKdfOneStepAuxFunction.KMAC_256, true, 512),
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

			ValidateAuxFunctions(parameters.AuxFunctions, errors);
			ValidateFixedInfoPattern(parameters.FixedInfoPattern, errors);
			ValidateFixedInfoEncoding(parameters.FixedInfoEncoding, errors);
			ValidateL(parameters, errors);
			
			return new ParameterValidateResponse(errors);
		}

		private void ValidateAlgoMode(Parameters parameters, List<string> errors)
		{
			try
			{
				var algoMode =
					AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

				if (algoMode != AlgoMode.KAS_KDF_OneStep)
				{
					errors.Add("Invalid algo/mode/revision for generator.");
				}
			}
			catch (Exception e)
			{
				errors.Add(e.Message);
			}
		}
		
		private void ValidateAuxFunctions(AuxFunction[] auxFunctions, List<string> errors)
		{
			if (auxFunctions == null || !auxFunctions.Any())
			{
				errors.Add("at least one AuxFunction is required.");
				return;
			}

			errors.AddIfNotNullOrEmpty(ValidateArray(
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
					errors.AddIfNotNullOrEmpty(
						ValidateArray(
							auxFunction.MacSaltMethods,
							new[] { MacSaltMethod.Default, MacSaltMethod.Random },
							"Salt Method OneStep KDF"));
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
			foreach (var auxFunctionName in param.AuxFunctions.Select(s => s.AuxFunctionName))
			{
				var auxFunctionMinimumL = ValidAuxMethods
					.First(w => w.functionName == auxFunctionName).minimumL;

				if (param.L < auxFunctionMinimumL)
				{
					errors.Add($"Provided 'l' value of {param.L} does not meet the minimum l value of {auxFunctionMinimumL} for the function {auxFunctionName}.");
				}
			}

			if (param.L > MaximumL)
			{
				errors.Add($"Provided 'l' value of {param.L} exceeds that maximum testable l value of {MaximumL}.");
			}
		}
	}
}