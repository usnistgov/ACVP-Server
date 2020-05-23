using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
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

			//Vector set must be in Failed status
			var vsStatus = _vectorSetService.GetStatus(payload.VectorSetID);
			if (vsStatus != VectorSetStatus.Failed)
			{
				errors.Add($"Vector set not in '{EnumHelpers.GetEnumDescriptionFromEnum(VectorSetStatus.Failed)}' status.");
				errors.Add($"Unable to submit results when vector set in '{EnumHelpers.GetEnumDescriptionFromEnum(vsStatus)}' status.");
			}

			// Environment check done by controller for if resubmission is allowed
			return new PayloadValidationResult(errors);
		}
	}
}