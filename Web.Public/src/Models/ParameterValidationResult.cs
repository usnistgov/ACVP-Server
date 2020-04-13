using System.Collections.Generic;

namespace Web.Public.Models
{
	public class ParameterValidationResult
	{
		public Dictionary<string, List<string>> ValidationErrors { get; }

		public bool IsSuccess => ValidationErrors?.Count == 0;

		public ParameterValidationResult(Dictionary<string, List<string>> validationErrors)
		{
			ValidationErrors = validationErrors;
		}
	}
}