using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		private static readonly AlgoMode[] ValidAlgoModes =
		{
			AlgoMode.KAS_ECC_SSC_Sp800_56Ar3,
			AlgoMode.KAS_FFC_SSC_Sp800_56Ar3
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

		private AlgoMode _algoMode;
		private bool _isKasEccRegistration;
		private bool _isKasFfcRegistration;

		public ParameterValidateResponse Validate(Parameters parameters)
		{
			var errors = new List<string>();

			SetAlgoModeProperties(parameters, errors);
			ValidateAlgoMode(_algoMode, errors);

			if (errors.Any())
			{
				return new ParameterValidateResponse(errors);
			}

			ValidateSchemes(_algoMode, parameters, errors);
			ValidateDomainParameterGeneration(_algoMode, parameters, errors);
			
			return new ParameterValidateResponse(errors);
		}

		private void SetAlgoModeProperties(Parameters parameters, List<string> errors)
		{
			_algoMode =
            	AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

			if (_algoMode == AlgoMode.KAS_ECC_SSC_Sp800_56Ar3)
			{
				_isKasEccRegistration = true;
			}

			if (_algoMode == AlgoMode.KAS_FFC_SSC_Sp800_56Ar3)
			{
				_isKasFfcRegistration = true;
			}
		}

		private void ValidateAlgoMode(AlgoMode algoMode, List<string> errors)
		{
			errors.AddIfNotNullOrEmpty(
				ValidateArray(new[] {algoMode}, ValidAlgoModes, "AlgoMode"));
		}		
		
		private void ValidateSchemes(AlgoMode algoMode, Parameters parameters, List<string> errors)
		{
			var schemesRegistered = parameters.Scheme.GetRegisteredSchemes().ToList();
			
			if (!schemesRegistered.Any())
			{
				errors.Add("No valid schemes registered.");
				return;
			}
			
			ValidateSchemesForAlgoMode(algoMode, schemesRegistered, errors);
		}
		
		private void ValidateSchemesForAlgoMode(AlgoMode algoMode, IEnumerable<SchemeBase> schemes, List<string> errors)
		{
			errors.AddRangeIfNotNullOrEmpty(
				from scheme in schemes 
				where scheme.AlgoMode != algoMode 
				select $"{nameof(algoMode)} {algoMode} is not valid with the {nameof(scheme)} {scheme.Scheme}.");
		}
		
		private void ValidateDomainParameterGeneration(AlgoMode algoMode, Parameters parameters, List<string> errors)
		{
			if (_isKasEccRegistration)
			{
				errors.AddIfNotNullOrEmpty(ValidateArray(parameters.DomainParameterGenerationMethods,
					ValidEccDpGeneration, nameof(parameters.DomainParameterGenerationMethods)));
			}
            
			if (_isKasFfcRegistration)
			{
				errors.AddIfNotNullOrEmpty(ValidateArray(parameters.DomainParameterGenerationMethods,
					ValidFfcDpGeneration, nameof(parameters.DomainParameterGenerationMethods)));
			}
		}
	}
}