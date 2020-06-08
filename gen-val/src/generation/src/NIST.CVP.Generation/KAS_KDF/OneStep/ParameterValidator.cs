using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
	{
		public ParameterValidateResponse Validate(Parameters parameters)
		{
			var errors = new List<string>();
			
			ValidateAlgoMode(parameters, errors);

			if (errors.Any())
			{
				return new ParameterValidateResponse(errors);
			}
			
			
			
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
	}
}