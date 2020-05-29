using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public interface IMessagePayloadValidator
	{
		PayloadValidationResult Validate(IMessagePayload messagePayload);
	}
}