using System.Collections.Generic;
using System.Linq;

namespace Web.Public.Results
{
	public class PayloadValidationResult
	{
		public List<string> Errors { get; }
		public bool IsSuccess => Errors == null || !Errors.Any();

		public PayloadValidationResult(List<string> errors)
		{
			Errors = errors;
		}
	}
}