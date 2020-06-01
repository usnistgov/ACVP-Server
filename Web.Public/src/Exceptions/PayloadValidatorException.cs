using System;
using System.Collections.Generic;

namespace Web.Public.Exceptions
{
	public class PayloadValidatorException : Exception, IMultiMessageException
	{
		public List<string> Errors { get; } = new List<string>();

		public PayloadValidatorException(string message) : base(message)
		{
			Errors.Add(message);
		}

		public PayloadValidatorException(List<string> errors)
		{
			Errors = errors;
		}
	}
}