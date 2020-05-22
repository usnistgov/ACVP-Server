using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class SubmitVectorSetResultsPayloadValidator : IMessagePayloadValidator
	{
		private readonly IVectorSetService _vectorSetService;

		public SubmitVectorSetResultsPayloadValidator(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}

		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var payload = (VectorSetSubmissionPayload) workflowItemPayload;
			var errors = new List<string>();

			//Vector set must be in Processed status
			if (_vectorSetService.GetStatus(payload.VectorSetID) != VectorSetStatus.Processed)
			{
				errors.Add("Vector set not in Processed status");
			}

			// Environment check done by controller
			return new PayloadValidationResult(errors);
		}
	}
}