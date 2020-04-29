using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class SubmitVectorSetResultsPayloadValidator : IMessagePayloadValidator
	{
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var payload = (VectorSetSubmissionPayload) workflowItemPayload;
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(payload.Algorithm))
			{
				errors.Add("algorithm not provided");
			}

			if (string.IsNullOrWhiteSpace(payload.Revision))
			{
				errors.Add("revision not provided");
			}

			// Environment check done by controller
			return new PayloadValidationResult(errors);
		}
	}
}