using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class ResubmitVectorSetResultsPayloadValidator : IMessagePayloadValidator
	{
		private readonly IVectorSetService _vectorSetService;

		public ResubmitVectorSetResultsPayloadValidator(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

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

			//Vector set must be in Failed status
			if (_vectorSetService.GetStatus(payload.VectorSetID) != Models.VectorSetStatus.Failed)
			{
				errors.Add("Vector set not in Failed status");
			}

			// Environment check done by controller for if resubmission is allowed
			return new PayloadValidationResult(errors);
		}
	}
}