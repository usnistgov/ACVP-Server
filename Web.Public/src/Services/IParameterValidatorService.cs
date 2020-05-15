using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IParameterValidatorService
	{
		ParameterValidationResult Validate(TestSessionRegisterPayload registration);
	}
}