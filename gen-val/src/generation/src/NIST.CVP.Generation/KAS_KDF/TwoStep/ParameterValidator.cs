using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Exceptions;

namespace NIST.CVP.Generation.KAS_KDF.TwoStep
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
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
		
		private static readonly MacSaltMethod[] ValidSaltGenerationMethods = { MacSaltMethod.Default, MacSaltMethod.Random };
		
		private static readonly FixedInfoEncoding[] ValidEncodingTypes = 
		{
			FixedInfoEncoding.Concatenation, FixedInfoEncoding.ConcatenationWithLengths
		};

		public ParameterValidateResponse Validate(Parameters parameters)
		{
			var errors = new List<string>();
			
			ValidateAlgoMode(parameters, errors);

			if (errors.Any())
			{
				return new ParameterValidateResponse(errors);
			}

			ValidateKdfMethod(parameters, errors);
			ValidateZ(parameters, errors);
			
			return new ParameterValidateResponse(errors);
		}
		
		private void ValidateAlgoMode(Parameters parameters, List<string> errors)
		{
			try
			{
				var algoMode =
					AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

				if (algoMode != AlgoMode.KAS_KDF_TwoStep_Sp800_56Cr1)
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
				ValidateFixedInfoPattern(capability.FixedInfoPattern, errorResults, null);
				ValidateEncoding(capability.FixedInfoEncoding, errorResults);
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
		
		private void ValidateEncoding(FixedInfoEncoding[] encoding, List<string> errorResults)
		{
			errorResults.AddIfNotNullOrEmpty(ValidateArray(encoding, ValidEncodingTypes, "Encoding type"));
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