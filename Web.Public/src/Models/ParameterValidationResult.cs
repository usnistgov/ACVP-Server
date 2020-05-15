using System.Collections.Generic;

namespace Web.Public.Models
{
	public class ParameterValidationResult
	{
		public List<string> ValidationErrors { get; }

		public bool IsSuccess => ValidationErrors?.Count == 0;

		public ParameterValidationResult(List<string> validationErrors)
		{
			ValidationErrors = validationErrors;
		}
	}
}