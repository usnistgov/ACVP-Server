using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLSv13.RFC8446
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		private static readonly AlgoMode ValidAlgoMode = AlgoMode.Tls_v1_3_v1_0;
		private static readonly List<HashFunctions> ValidHashFunctions = EnumHelpers.GetEnumsWithoutDefault<HashFunctions>();
		
		public ParameterValidateResponse Validate(Parameters parameters)
		{
			List<string> errors = new List<string>();

			ValidateAlgoModeRevision(parameters, errors);
			ValidateHashAlgs(parameters.HmacAlg, errors);
			
			return new ParameterValidateResponse(errors);
		}

		private void ValidateAlgoModeRevision(Parameters parameters, List<string> errors)
		{
			ValidateAlgoMode(parameters, new[] { ValidAlgoMode }, errors);
		}
		
		private void ValidateHashAlgs(HashFunctions[] parametersHashAlg, List<string> errors)
		{
			errors.AddIfNotNullOrEmpty(
				ValidateArray(parametersHashAlg, ValidHashFunctions, "hashAlg"));
		}

	}
}