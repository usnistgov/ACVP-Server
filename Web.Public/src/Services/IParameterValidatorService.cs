using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Models;

namespace Web.Public.Services
{
	public interface IParameterValidatorService
	{
		ParameterValidationResult Validate(TestSessionRegisterPayload registration);
	}
}