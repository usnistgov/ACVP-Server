using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Schema;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		private static readonly AlgoMode[] ValidAlgoModes =
		{
			AlgoMode.KAS_ECC_SSC_Sp800_56Ar3,
			AlgoMode.KAS_FFC_SSC_Sp800_56Ar3
		};

		public static readonly KeyAgreementRole[] ValidKeyAgreementRoles =
		{
			KeyAgreementRole.InitiatorPartyU,
			KeyAgreementRole.ResponderPartyV
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
			KasDpGeneration.P224,
			KasDpGeneration.P256,
			KasDpGeneration.P384,
			KasDpGeneration.P521,
			KasDpGeneration.K233,
			KasDpGeneration.K283,
			KasDpGeneration.K409,
			KasDpGeneration.K571,
			KasDpGeneration.B233,
			KasDpGeneration.B283,
			KasDpGeneration.B409,
			KasDpGeneration.B571,
		};
		
		private static readonly Dictionary<KasDpGeneration, int> DpGenerationEstimatedSecurityStrengths = new Dictionary<KasDpGeneration, int>()
		{
			{ KasDpGeneration.Modp2048, 103},
			{ KasDpGeneration.Modp3072, 125},
			{ KasDpGeneration.Modp4096, 150},
			{ KasDpGeneration.Modp6144, 175},
			{ KasDpGeneration.Modp8192, 192},
			{ KasDpGeneration.Ffdhe2048, 103},
			{ KasDpGeneration.Ffdhe3072, 125},
			{ KasDpGeneration.Ffdhe4096, 150},
			{ KasDpGeneration.Ffdhe6144, 175},
			{ KasDpGeneration.Ffdhe8192, 192},
			{ KasDpGeneration.Fb, 112},
			{ KasDpGeneration.Fc, 112},
			{ KasDpGeneration.P192, 80},
			{ KasDpGeneration.P224, 112},
			{ KasDpGeneration.P256, 128},
			{ KasDpGeneration.P384, 192},
			{ KasDpGeneration.P521, 256},
			{ KasDpGeneration.K163, 112},
			{ KasDpGeneration.K233, 128},
			{ KasDpGeneration.K283, 128},
			{ KasDpGeneration.K409, 192},
			{ KasDpGeneration.K571, 256},
			{ KasDpGeneration.B233, 128},
			{ KasDpGeneration.B283, 128},
			{ KasDpGeneration.B409, 192},
			{ KasDpGeneration.B571, 256},
		};
		
		private static readonly Dictionary<HashFunctions, int> HashFunctionEstimatedSecurityStrengths = new Dictionary<HashFunctions, int>()
		{
			{ HashFunctions.Sha1, 80 },
			{ HashFunctions.Sha2_d224, 112 },
			{ HashFunctions.Sha2_d256, 128 },
			{ HashFunctions.Sha2_d384, 192 },
			{ HashFunctions.Sha2_d512, 256 },
			{ HashFunctions.Sha2_d512t224, 112 },
			{ HashFunctions.Sha2_d512t256, 128 },
			{ HashFunctions.Sha3_d224, 112 },
			{ HashFunctions.Sha3_d256, 128 },
			{ HashFunctions.Sha3_d384, 192 },
			{ HashFunctions.Sha3_d512, 256 },
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
			ValidateHashDomainParamGenerationSecurity(parameters, errors);
			
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
			foreach (var scheme in schemesRegistered)
			{
				var schemeRoles = scheme.KasRole;
				errors.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
			}
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
		
		private void ValidateHashDomainParamGenerationSecurity(Parameters parameters, List<string> errors)
		{
			// If we're not hashing Z, we can't compare security strengths of the domain parameter generation to the hash.
			if (parameters.HashFunctionZ == HashFunctions.None)
			{
				return;
			}
			
			// Need to ensure that the registered hash's security strength can be covered within the registered domain parameter security strengths.
			var registeredDpJoinedWithSecurityStrengths =
				from registeredDp in parameters.DomainParameterGenerationMethods
				join validDps in DpGenerationEstimatedSecurityStrengths on registeredDp equals validDps.Key
				select new
				{
					DpGenType = registeredDp,
					SecurityStrength = validDps.Value
				};

			var hash = HashFunctionEstimatedSecurityStrengths.First(w => w.Key == parameters.HashFunctionZ);

			errors.AddRange(
				registeredDpJoinedWithSecurityStrengths
					.Where(x => x.SecurityStrength > hash.Value)
					.Select(registeredDp => 
						$"{nameof(hash)} {hash.Key} is invalid as its security strength is too low to be used in conjunction with {registeredDp.DpGenType}"));
		}
	}
}