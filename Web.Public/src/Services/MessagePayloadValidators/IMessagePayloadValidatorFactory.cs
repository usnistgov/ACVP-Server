using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Services.MessagePayloadValidators
{
	public interface IMessagePayloadValidatorFactory
	{
		IMessagePayloadValidator GetMessagePayloadValidator(APIAction action);
	}
}